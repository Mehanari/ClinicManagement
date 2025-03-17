using ClinicManagement.Context;
using ClinicManagement.Models.Entities;
using ClinicManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Controllers
{
    [Authorize(Roles = "Реєстратор")]
    public class RegistrarController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IMedicalCardService _medicalCardService;

        public RegistrarController(MyDbContext context, IMedicalCardService medicalCardService)
        {
            _context = context;
            _medicalCardService = medicalCardService;
        }

        [HttpGet]
        public IActionResult CreateMedicalCard()
        {
            ViewData["Genders"] = _context.Genders.ToList();
            ViewData["IdentificationTypes"] = _context.IdentificationTypes.ToList();
            return View(new CreateMedicalCardViewModel());
        }

        [HttpPost]
        public IActionResult CreateMedicalCard(CreateMedicalCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var personalInfo = new PersonalInfo
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Patronymic = model.Patronymic,
                    DateOfBirth = model.DateOfBirth,
                    GenderId = model.GenderId
                };
                _context.PersonalInfos.Add(personalInfo);
                _context.SaveChanges();

                var identification = new Identification
                {
                    Identifier = model.Identifier,
                    IdentificationTypeId = model.IdentificationTypeId
                };
                _context.Identifications.Add(identification);
                _context.SaveChanges();

                var medicalCard = new MedicalCard
                {
                    PersonalInfoId = personalInfo.Id,
                    Workplace = model.Workplace,
                    Address = model.Address,
                    Email = model.Email,
                    Phone = model.Phone,
                    IdentificationId = identification.Id
                };
                _context.MedicalCards.Add(medicalCard);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Медична картка успішно створена!";
                return RedirectToAction("SearchMedicalCards");
            }

            ViewData["Genders"] = _context.Genders.ToList();
            ViewData["IdentificationTypes"] = _context.IdentificationTypes.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult SearchMedicalCards()
        {
            return View(new SearchMedicalCardViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SearchMedicalCards(SearchMedicalCardViewModel model)
        {
            return View(await _medicalCardService.SearchMedicalCardsAsync(model, _context));
        }

        [HttpGet]
        public IActionResult CreateAppointment(int id)
        {
            var card = _context.MedicalCards.FirstOrDefault(mc => mc.Number == id);
            if (card == null)
            {
                return NotFound();
            }

            ViewData["DoctorSpecialities"] = _context.DoctorSpecialities.ToList();
            var model = new CreateAppointmentViewModel
            {
                CardNumber = id
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateAppointment(CreateAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = new Appointment
                {
                    CardNumber = model.CardNumber,
                    DoctorSpecialityId = model.DoctorSpecialityId,
                    StartTime = model.StartTime,
                    EndTime = model.StartTime.AddMinutes(model.DurationMinutes),
                    RoomNumber = model.RoomNumber
                };
                _context.Appointments.Add(appointment);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Запис на прийом успішно створено!";
                return RedirectToAction("SearchMedicalCards");
            }

            ViewData["DoctorSpecialities"] = _context.DoctorSpecialities.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewSchedule()
        {
            var model = new ViewScheduleViewModel();
            var query = _context.Appointments
                .Include(a => a.DoctorSpeciality)
                .Join(_context.MedicalCards,
                    a => a.CardNumber,
                    mc => mc.Number,
                    (a, mc) => new { Appointment = a, MedicalCard = mc })
                .Join(_context.PersonalInfos,
                    x => x.MedicalCard.PersonalInfoId,
                    pi => pi.Id,
                    (res, pi) => new { res.Appointment, res.MedicalCard, PersonalInfo = pi });

            var results = query.ToList();
            model.Appointments = results.Select(x => x.Appointment).ToList();
            var medicalCards = results.Select(x => x.MedicalCard).ToList();
            var personalInfos = results.Select(x => x.PersonalInfo).ToList();

            ViewData["PersonalInfos"] = GetPersonalInfosDict(model.Appointments, medicalCards, personalInfos);
            ViewData["DoctorSpecialities"] = _context.DoctorSpecialities.ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult ViewSchedule(ViewScheduleViewModel model)
        {
            var query = _context.Appointments
                .Include(a => a.DoctorSpeciality)
                .Join(_context.MedicalCards,
                    a => a.CardNumber,
                    mc => mc.Number,
                    (a, mc) => new { Appointment = a, MedicalCard = mc })
                .Join(_context.PersonalInfos,
                    x => x.MedicalCard.PersonalInfoId,
                    pi => pi.Id,
                    (x, pi) => new { x.Appointment, x.MedicalCard, PersonalInfo = pi })
                .AsQueryable();
            

            if (model.FilterDate.HasValue)
            {
                var filterDate = model.FilterDate.Value.Date;
                query = query.Where(x => x.Appointment.StartTime.Date == filterDate);
            }

            if (model.FilterSpecialityId.HasValue)
            {
                query = query.Where(x => x.Appointment.DoctorSpecialityId == model.FilterSpecialityId.Value);
            }

            var result = query.ToList();
            var medicalCards = result.Select(x => x.MedicalCard).ToList();
            var personalInfos = result.Select(x => x.PersonalInfo).ToList();
            var appointments = result.Select(x => x.Appointment).ToList();
            var dict = GetPersonalInfosDict(appointments, medicalCards, personalInfos);
            model.Appointments = appointments;
            ViewData["PersonalInfos"] = dict;
            ViewData["DoctorSpecialities"] = _context.DoctorSpecialities.ToList();
            return View(model);
        }

        private Dictionary<int, PersonalInfo> GetPersonalInfosDict(List<Appointment> appointments,
            List<MedicalCard> medicalCards, List<PersonalInfo> personalInfos)
        {
            var dict = new Dictionary<int, PersonalInfo>();
            foreach (var appointment in appointments)
            {
                var medicalCard = medicalCards.FirstOrDefault(m => m.Number == appointment.CardNumber);
                if (medicalCard is null)
                {
                    continue;
                }
                var personalInfo = personalInfos
                    .First(p => p.Id == medicalCard.PersonalInfoId);
                var cardNumber = appointment.CardNumberNavigation.Number;
                dict.TryAdd(cardNumber, personalInfo);
            }

            return dict;
        }

        [HttpGet]
        public IActionResult EditAppointment(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewData["DoctorSpecialities"] = _context.DoctorSpecialities.ToList();
            var model = new EditAppointmentViewModel
            {
                Id = appointment.Id,
                CardNumber = appointment.CardNumber,
                DoctorSpecialityId = appointment.DoctorSpecialityId,
                StartTime = appointment.StartTime,
                DurationMinutes = (int)(appointment.EndTime - appointment.StartTime).TotalMinutes,
                RoomNumber = appointment.RoomNumber
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditAppointment(EditAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appointment = _context.Appointments.FirstOrDefault(a => a.Id == model.Id);
                if (appointment == null)
                {
                    return NotFound();
                }

                appointment.CardNumber = model.CardNumber;
                appointment.DoctorSpecialityId = model.DoctorSpecialityId;
                appointment.StartTime = model.StartTime;
                appointment.EndTime = model.StartTime.AddMinutes(model.DurationMinutes);
                appointment.RoomNumber = model.RoomNumber;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Запис на прийом успішно оновлено!";
                return RedirectToAction("ViewSchedule");
            }

            ViewData["DoctorSpecialities"] = _context.DoctorSpecialities.ToList();
            return View(model);
        }
    }
}