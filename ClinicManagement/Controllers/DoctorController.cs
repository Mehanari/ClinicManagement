using System.Security.Claims;
using ClinicManagement.Context;
using ClinicManagement.Models.Entities;
using ClinicManagement.Models.ViewModels;
using ClinicManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Controllers
{
    [Authorize(Roles = "Лікар")]
    public class DoctorController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IMedicalCardService _medicalCardService;
        private readonly IScheduleService _scheduleService;

        public DoctorController(MyDbContext context, IMedicalCardService medicalCardService, IScheduleService scheduleService)
        {
            _context = context;
            _medicalCardService = medicalCardService;
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public IActionResult SearchMedicalCards()
        {
            return View(new SearchMedicalCardViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SearchMedicalCards(SearchMedicalCardViewModel model)
        {
            model = await _medicalCardService.SearchMedicalCardsAsync(model, _context);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewSchedule()
        {
            return View(await _scheduleService.GetSchedule(new ViewScheduleViewModel(), _context));
        }

        [HttpPost]
        public async Task<IActionResult> ViewSchedule(ViewScheduleViewModel model)
        {
            return View(await _scheduleService.GetSchedule(model, _context));
        }
        
        [HttpGet]
        public IActionResult AddAppointmentResult(int id)
        {
            var medicalCard = _context.MedicalCards.FirstOrDefault(mc => mc.Number == id);
            if (medicalCard == null)
            {
                return NotFound();
            }
            
            var availableAppointments = _context.Appointments
                .Where(a => a.CardNumber == id && a.StartTime.Date == DateTime.Today)
                .ToList();
            
            var model = new AddAppointmentResultViewModel
            {
                MedicalCardNumber = id,
                AvailableAppointments = availableAppointments
            };

            return View(model);
        }
        
        [HttpPost]
        public IActionResult AddAppointmentResult(AddAppointmentResultViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized("Id користувача не знайдено.");
                }
                var doctor = _context.Doctors.FirstOrDefault(d => d.Id == int.Parse(currentUserId)); 
                if (doctor == null)
                {
                    return Unauthorized("Лікар не знайдений.");
                }

                var appointmentResult = new AppointmentResult
                {
                    AppointmentId = model.AppointmentId.Value,
                    DoctorId = doctor.Id,
                    Reason = model.Reason,
                    Anamnesis = model.Anamnesis,
                    Objectively = model.Objectively,
                    RadiationDose = model.RadiationDose,
                    Diagnosis = model.Diagnosis,
                    Prescription = model.Prescription,
                    Recommendations = model.Recommendations,
                    Actions = model.Actions,
                    Conclusion = model.Conclusion
                };

                _context.AppointmentResults.Add(appointmentResult);

                // Оновлення останнього діагнозу в медичній картці
                var medicalCard = _context.MedicalCards.FirstOrDefault(mc => mc.Number == model.MedicalCardNumber);
                if (medicalCard != null)
                {
                    medicalCard.LastDiagnosis = model.Diagnosis;
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Результат прийому успішно додано!";
                return RedirectToAction("SearchMedicalCards");
            }

            // Перезавантажуємо доступні записи на прийом у разі помилки
            model.AvailableAppointments = _context.Appointments
                .Where(a => a.CardNumber == model.MedicalCardNumber && a.StartTime <= DateTime.Now && a.EndTime >= DateTime.Now)
                .ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewMedicalCard(int id)
        {
            var medicalCard = _context.MedicalCards
                .FirstOrDefault(mc => mc.Number == id);
            
            if (medicalCard == null)
            {
                return NotFound();
            }

            var appointments = _context.Appointments
                .Include(a => a.DoctorSpeciality)
                .Where(a => a.CardNumber == id)
                .ToList();

            var appointmentResults = _context.AppointmentResults
                .Include(ar => ar.Appointment)
                .Include(ar => ar.Doctor)
                .ThenInclude(d => d.Speciality)
                .Where(ar => ar.Appointment.CardNumber == id)
                .ToList();

            var personalInfo = _context.PersonalInfos.FirstOrDefault(pi => pi.Id == medicalCard.PersonalInfoId);
            
            if (personalInfo == null)
            {
                return NotFound();
            }
            
            var model = new ViewMedicalCardViewModel
            {
                MedicalCard = medicalCard,
                Appointments = appointments,
                AppointmentResults = appointmentResults,
                PersonalInfo = personalInfo
            };

            return View(model);
        }
    }
}