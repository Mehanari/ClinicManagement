using ClinicManagement.Models.Entities;

namespace ClinicManagement.Models.ViewModels;

public class SearchDoctorViewModel
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
    public int? SpecialityId { get; set; }

    public List<DoctorSearchResult> Results { get; set; } = new List<DoctorSearchResult>();
    public Dictionary<int, PersonalInfo> PersonalInfos { get; set; } = new Dictionary<int, PersonalInfo>();
    public List<DoctorSpeciality> Specialities { get; set; } = new List<DoctorSpeciality>();
}

public class DoctorSearchResult
{
    public int UserId { get; set; }
    public int PersonalInfoId { get; set; }
    public int SpecialityId { get; set; }
    public string SpecialityName { get; set; }
}