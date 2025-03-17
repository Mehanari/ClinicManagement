using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть логін")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}