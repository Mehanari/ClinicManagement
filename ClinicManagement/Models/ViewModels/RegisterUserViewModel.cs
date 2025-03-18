using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models.ViewModels;

public class RegisterUserViewModel
{
    [Required(ErrorMessage = "Ім’я є обов’язковим")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Прізвище є обов’язковим")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "По батькові є обов’язковим")]
    public string Patronymic { get; set; }

    [Required(ErrorMessage = "Дата народження є обов’язковою")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Стать є обов’язковою")]
    public int GenderId { get; set; }

    [Required(ErrorMessage = "Логін є обов’язковим")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Пароль є обов’язковим")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Роль є обов’язковою")]
    public int RoleId { get; set; }

    public int? SpecialityId { get; set; } // Optional, only for doctors
}