using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models.ViewModels
{
    public class CreateAppointmentViewModel
    {
        public int CardNumber { get; set; }

        [Required(ErrorMessage = "Спеціальність лікаря є обов’язковою")]
        public int DoctorSpecialityId { get; set; }

        [Required(ErrorMessage = "Дата і час початку є обов’язковими")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Тривалість є обов’язковою")]
        [Range(15, 120, ErrorMessage = "Тривалість повинна бути від 15 до 120 хвилин")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Номер кабінету є обов’язковим")]
        [Range(1, 999, ErrorMessage = "Номер кабінету повинен бути від 1 до 999")]
        public int RoomNumber { get; set; }
    }
}