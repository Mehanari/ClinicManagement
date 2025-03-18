using ClinicManagement.Models.Entities;

namespace ClinicManagement.Models.ViewModels
{
    public class ViewScheduleViewModel
    {
        public DateTime? FilterDate { get; set; }
        public int? FilterSpecialityId { get; set; }
        public int? FilterRoomNumber { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public Dictionary<int, PersonalInfo> PersonalInfos { get; set; } = new Dictionary<int, PersonalInfo>();
        public List<DoctorSpeciality> DoctorSpecialities { get; set; } = new List<DoctorSpeciality>();
    }
}