namespace ClinicManagement.Models.Entities;

public partial class IdentificationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Identification> Identifications { get; set; } = new List<Identification>();
}
