using ClinicManagement.Models.Entities;
using ClinicManagement.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public class ScheduleService : IScheduleService
{
    public async Task<ViewScheduleViewModel> GetSchedule(ViewScheduleViewModel model, DbContext context)
    {
        var query = context.Set<Appointment>()
            .Include(a => a.DoctorSpeciality)
            .Join(context.Set<MedicalCard>(),
                a => a.CardNumber,
                mc => mc.Number,
                (a, mc) => new { Appointment = a, MedicalCard = mc })
            .Join(context.Set<PersonalInfo>(),
                x => x.MedicalCard.PersonalInfoId,
                pi => pi.Id,
                (x, pi) => new { x.Appointment, x.MedicalCard, PersonalInfo = pi })
            .AsQueryable();
            

        if (model.FilterDate.HasValue)
        {
            var filterDate = model.FilterDate.Value.Date;
            query = query.Where(x => x.Appointment.StartTime.Date == filterDate);
        }

        if (model.FilterSpecialityId.HasValue)
        {
            query = query.Where(x => x.Appointment.DoctorSpecialityId == model.FilterSpecialityId.Value);
        }

        if (model.FilterRoomNumber.HasValue)
        {
            query = query.Where(x => x.Appointment.RoomNumber == model.FilterRoomNumber.Value);
        }

        var result = await query.ToListAsync();
        var medicalCards = result.Select(x => x.MedicalCard).ToList();
        var personalInfos = result.Select(x => x.PersonalInfo).ToList();
        var appointments = result.Select(x => x.Appointment).ToList();
        var dict = GetPersonalInfosDict(appointments, medicalCards, personalInfos);
        model.Appointments = appointments;
        model.PersonalInfos = dict;
        model.DoctorSpecialities = context.Set<DoctorSpeciality>().ToList();
        return model;
    }
    
    private Dictionary<int, PersonalInfo> GetPersonalInfosDict(List<Appointment> appointments,
        List<MedicalCard> medicalCards, List<PersonalInfo> personalInfos)
    {
        var dict = new Dictionary<int, PersonalInfo>();
        foreach (var appointment in appointments)
        {
            var medicalCard = medicalCards.FirstOrDefault(m => m.Number == appointment.CardNumber);
            if (medicalCard is null)
            {
                continue;
            }
            var personalInfo = personalInfos
                .First(p => p.Id == medicalCard.PersonalInfoId);
            var cardNumber = appointment.CardNumberNavigation.Number;
            dict.TryAdd(cardNumber, personalInfo);
        }

        return dict;
    }
}