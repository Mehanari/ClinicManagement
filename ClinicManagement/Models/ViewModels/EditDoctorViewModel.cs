using ClinicManagement.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models.ViewModels
{
    public class EditDoctorViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Прізвище є обов’язковим")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Ім’я є обов’язковим")]
        public string Name { get; set; }

        [Required(ErrorMessage = "По батькові є обов’язковим")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Спеціальність є обов’язковою")]
        public int SpecialityId { get; set; }

        public List<DoctorSpeciality> Specialities { get; set; } = new List<DoctorSpeciality>();
    }
}