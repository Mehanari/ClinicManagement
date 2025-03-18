using System.ComponentModel.DataAnnotations;
using ClinicManagement.Models.Entities;

namespace ClinicManagement.Models.ViewModels
{
    public class AddAppointmentResultViewModel
    {
        [Required(ErrorMessage = "Номер медичної картки є обов’язковим")]
        public int MedicalCardNumber { get; set; }

        [Required(ErrorMessage = "Виберіть запис на прийом")]
        public int? AppointmentId { get; set; }
        public List<Appointment> AvailableAppointments { get; set; } = new List<Appointment>();

        [Required(ErrorMessage = "Причина є обов’язковою")]
        [StringLength(500, ErrorMessage = "Причина не може перевищувати 500 символів")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Анамнез є обов’язковим")]
        [StringLength(1000, ErrorMessage = "Анамнез не може перевищувати 1000 символів")]
        public string Anamnesis { get; set; }

        [Required(ErrorMessage = "Об’єктивні дані є обов’язковими")]
        [StringLength(1000, ErrorMessage = "Об’єктивні дані не можуть перевищувати 1000 символів")]
        public string Objectively { get; set; }

        [Required(ErrorMessage = "Діагноз є обов’язковим")]
        [StringLength(500, ErrorMessage = "Діагноз не може перевищувати 500 символів")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Призначення є обов’язковим")]
        [StringLength(1000, ErrorMessage = "Призначення не може перевищувати 1000 символів")]
        public string Prescription { get; set; }

        [Required(ErrorMessage = "Рекомендації є обов’язковими")]
        [StringLength(1000, ErrorMessage = "Рекомендації не можуть перевищувати 1000 символів")]
        public string Recommendations { get; set; }

        [Required(ErrorMessage = "Дії є обов’язковими")]
        [StringLength(1000, ErrorMessage = "Дії не можуть перевищувати 1000 символів")]
        public string Actions { get; set; }

        [Required(ErrorMessage = "Висновок є обов’язковим")]
        [StringLength(500, ErrorMessage = "Висновок не може перевищувати 500 символів")]
        public string Conclusion { get; set; }

        [Required(ErrorMessage = "Доза опромінення є обов’язковою")]
        [Range(0, double.MaxValue, ErrorMessage = "Доза опромінення повинна бути додатньою")]
        public decimal RadiationDose { get; set; }
    }
}