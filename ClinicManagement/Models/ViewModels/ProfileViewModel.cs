using ClinicManagement.Models.Entities;

namespace ClinicManagement.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string Login { get; set; }
        public string Role { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
    }
}