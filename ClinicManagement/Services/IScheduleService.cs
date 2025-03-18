using ClinicManagement.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Services;

public interface IScheduleService
{
    Task<ViewScheduleViewModel> GetSchedule(ViewScheduleViewModel model, DbContext context);

}