using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models.Entities;

public partial class PersonalInfo
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Ім’я є обов’язковим")]
    public string Name { get; set; } = null!;
    
    [Required(ErrorMessage = "Прізвище є обов’язковим")]
    public string Surname { get; set; } = null!;
    
    [Required(ErrorMessage = "По батьковій є обов’язковим")]
    public string Patronymic { get; set; } = null!;
    
    [Required(ErrorMessage = "Дата народження є обов’язковою")]
    public DateOnly DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "Стать є обов’язковою")]
    public int GenderId { get; set; }
    public virtual Gender Gender { get; set; } = null!;
}
