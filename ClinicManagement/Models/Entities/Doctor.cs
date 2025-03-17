namespace ClinicManagement.Models.Entities;

public partial class Doctor
{
    public int Id { get; set; }

    public int SpecialityId { get; set; }

    public virtual ICollection<AppointmentResult> AppointmentResults { get; set; } = new List<AppointmentResult>();

    public virtual DoctorSpeciality Speciality { get; set; } = null!;
}
