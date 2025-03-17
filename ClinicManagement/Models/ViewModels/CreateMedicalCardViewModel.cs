using System.ComponentModel.DataAnnotations;
using ClinicManagement.Models.Entities;

namespace ClinicManagement.Models.ViewModels
{
    public class CreateMedicalCardViewModel
    {
        // PersonalInfo
        [Required(ErrorMessage = "Ім’я є обов’язковим")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Прізвище є обов’язковим")]
        public string Surname { get; set; }

        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Дата народження є обов’язковою")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Стать є обов’язковою")]
        public int GenderId { get; set; }

        // Identification
        [Required(ErrorMessage = "Ідентифікатор є обов’язковим")]
        public string Identifier { get; set; }

        [Required(ErrorMessage = "Тип ідентифікації є обов’язковим")]
        public int IdentificationTypeId { get; set; }

        // MedicalCard
        [Required(ErrorMessage = "Місце роботи є обов’язковим")]
        public string Workplace { get; set; }

        [Required(ErrorMessage = "Адреса є обов’язковою")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Електронна пошта є обов’язковою")]
        [EmailAddress(ErrorMessage = "Невірний формат електронної пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Телефон є обов’язковим")]
        [Phone(ErrorMessage = "Невірний формат телефону")]
        public string Phone { get; set; }
    }
}