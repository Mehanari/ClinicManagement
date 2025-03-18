namespace ClinicManagement.Models.Entities;

public partial class MedicalCard
{
    public int Number { get; set; }

    public int PersonalInfoId { get; set; }

    public string Workplace { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? LastDiagnosis { get; set; }

    public int IdentificationId { get; set; }
    
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
