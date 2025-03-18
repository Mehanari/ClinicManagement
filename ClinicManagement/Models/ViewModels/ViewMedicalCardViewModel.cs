using ClinicManagement.Models.Entities;

namespace ClinicManagement.Models.ViewModels
{
    public class ViewMedicalCardViewModel
    {
        public MedicalCard MedicalCard { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<AppointmentResult> AppointmentResults { get; set; } = new List<AppointmentResult>();
    }
}