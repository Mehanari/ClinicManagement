using ClinicManagement.Context;
using ClinicManagement.Models.Entities;
using ClinicManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicManagement.Services;

namespace ClinicManagement.Controllers
{
    [Authorize(Roles = "Реєстратор")]
    public class RegistrarController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IMedicalCardService _medicalCardService;
        private readonly IScheduleService _scheduleService;

        public RegistrarController(MyDbContext context, IMedicalCardService medicalCardService, IScheduleService scheduleService)
        {
            _context = context;
            _medicalCardService = medicalCardService;
            _scheduleService = scheduleService;
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
        public async Task<IActionResult> ViewSchedule()
        {
            var model = new ViewScheduleViewModel();
            return View(await _scheduleService.GetSchedule(model, _context));
        }

        [HttpPost]
        public async Task<IActionResult> ViewSchedule(ViewScheduleViewModel model)
        {
            return View(await _scheduleService.GetSchedule(model, _context));
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