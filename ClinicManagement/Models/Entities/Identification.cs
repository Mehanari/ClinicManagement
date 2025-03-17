namespace ClinicManagement.Models.Entities;

public partial class Identification
{
    public int Id { get; set; }

    public string Identifier { get; set; } = null!;

    public int IdentificationTypeId { get; set; }

    public virtual IdentificationType IdentificationType { get; set; } = null!;
}
