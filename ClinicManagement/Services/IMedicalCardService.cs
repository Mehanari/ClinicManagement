using ClinicManagement.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services
{
    public interface IMedicalCardService
    {
        Task<SearchMedicalCardViewModel> SearchMedicalCardsAsync(SearchMedicalCardViewModel model, DbContext context);
    }
}