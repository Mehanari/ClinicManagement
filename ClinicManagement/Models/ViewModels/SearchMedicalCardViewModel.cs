using ClinicManagement.Models.Entities;

namespace ClinicManagement.Models.ViewModels
{
    public class SearchMedicalCardViewModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public int? Number { get; set; }
        public List<MedicalCard> Results { get; set; } = new List<MedicalCard>();
        public Dictionary<int, PersonalInfo> PersonalInfos { get; set; } = new Dictionary<int, PersonalInfo>();
    }
}