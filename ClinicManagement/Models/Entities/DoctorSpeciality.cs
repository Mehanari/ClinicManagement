namespace ClinicManagement.Models.Entities;

public partial class DoctorSpeciality
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
