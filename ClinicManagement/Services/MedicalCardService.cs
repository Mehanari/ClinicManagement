using ClinicManagement.Models.Entities;
using ClinicManagement.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services
{
    public class MedicalCardService : IMedicalCardService
    {
        public async Task<SearchMedicalCardViewModel> SearchMedicalCardsAsync(SearchMedicalCardViewModel model, DbContext context)
        {
            var query = context.Set<MedicalCard>()
                .Join(context.Set<PersonalInfo>(),
                    mc => mc.PersonalInfoId,
                    pi => pi.Id,
                    (mc, pi) => new { MedicalCard = mc, PersonalInfo = pi })
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                query = query.Where(x => x.PersonalInfo.Name.Contains(model.Name));
            }

            if (!string.IsNullOrEmpty(model.Surname))
            {
                query = query.Where(x => x.PersonalInfo.Surname.Contains(model.Surname));
            }

            if (!string.IsNullOrEmpty(model.Patronymic))
            {
                query = query.Where(x => x.PersonalInfo.Patronymic.Contains(model.Patronymic));
            }

            if (model.Number.HasValue)
            {
                query = query.Where(x => x.MedicalCard.Number == model.Number.Value);
            }

            var results = await query.ToListAsync();
            model.Results = results.Select(x => x.MedicalCard).ToList();
            model.PersonalInfos = results.ToDictionary(x => x.MedicalCard.PersonalInfoId, x => x.PersonalInfo);
            return model;
        }
    }
}