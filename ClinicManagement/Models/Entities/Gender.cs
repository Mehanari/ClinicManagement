namespace ClinicManagement.Models.Entities;

public partial class Gender
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PersonalInfo> PersonalInfos { get; set; } = new List<PersonalInfo>();
}
