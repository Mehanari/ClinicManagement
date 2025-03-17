namespace ClinicManagement.Models.Entities;

public partial class AppointmentResult
{
    public int Id { get; set; }

    public string Reason { get; set; } = null!;

    public string Anamnesis { get; set; } = null!;

    public string Objectively { get; set; } = null!;

    public decimal RadiationDose { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string Prescription { get; set; } = null!;

    public string Recommendations { get; set; } = null!;

    public string Actions { get; set; } = null!;

    public string Conclusion { get; set; } = null!;

    public int AppointmentId { get; set; }

    public int DoctorId { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;
}
