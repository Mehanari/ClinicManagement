namespace ClinicManagement.Models.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public int PersonalInfoId { get; set; }

    public virtual Role Role { get; set; } = null!;
}
