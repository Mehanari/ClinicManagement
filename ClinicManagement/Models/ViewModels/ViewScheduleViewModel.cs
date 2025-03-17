using ClinicManagement.Models.Entities;

namespace ClinicManagement.Models.ViewModels
{
    public class ViewScheduleViewModel
    {
        public DateTime? FilterDate { get; set; }
        public int? FilterSpecialityId { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}