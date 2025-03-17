namespace ClinicManagement.Models.Entities;

public partial class Appointment
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int RoomNumber { get; set; }

    public int CardNumber { get; set; }

    public int DoctorSpecialityId { get; set; }

    public virtual ICollection<AppointmentResult> AppointmentResults { get; set; } = new List<AppointmentResult>();

    public virtual MedicalCard CardNumberNavigation { get; set; } = null!;

    public virtual DoctorSpeciality DoctorSpeciality { get; set; } = null!;
}
