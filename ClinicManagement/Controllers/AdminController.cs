using ClinicManagement.Context;
using ClinicManagement.Exceptions;
using ClinicManagement.Models.Entities;
using ClinicManagement.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Controllers;

public class AdminController : Controller
{
    private readonly MyDbContext _context;

    public AdminController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult RegisterUser()
    {
        SetUpViewBag();
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            SetUpViewBag();
            return View(model);
        }

        try
        {
            // Start a transaction to ensure data consistency
            using var transaction = await _context.Database.BeginTransactionAsync();

            // Step 1: Create PersonalInfo
            var personalInfo = new PersonalInfo
            {
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                DateOfBirth = model.DateOfBirth,
                GenderId = model.GenderId
            };
            _context.PersonalInfos.Add(personalInfo);
            await _context.SaveChangesAsync();

            // Step 2: Create User
            var user = new User
            {
                Login = model.Login,
                Password = model.Password, 
                RoleId = model.RoleId,
                PersonalInfoId = personalInfo.Id
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Step 3: If role is "Doctor", create Doctor entity
            var doctorRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Лікар");
            if (doctorRole != null && model.RoleId == doctorRole.Id && model.SpecialityId.HasValue)
            {
                var doctor = new Doctor
                {
                    Id = user.Id,
                    SpecialityId = model.SpecialityId.Value
                };
                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return RedirectToAction("SearchDoctors");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error occurred: {ex.Message}");
            SetUpViewBag();
            return View(model);
        }
    }
    
    private void SetUpViewBag()
    {
        ViewBag.Roles = _context.Roles.ToList();
        ViewBag.Genders = _context.Genders.ToList();
        ViewBag.Specialities = _context.DoctorSpecialities.ToList();
        ViewBag.DoctorRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Лікар").Id;
    }
    
    [HttpGet]
    public IActionResult SearchDoctors()
    {
        var model = new SearchDoctorViewModel
        {
            Specialities = _context.DoctorSpecialities.ToList()
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SearchDoctors(SearchDoctorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Specialities = _context.DoctorSpecialities.ToList();
            return View(model);
        }

        var query = from doctor in _context.Doctors
                    join user in _context.Users on doctor.Id equals user.Id
                    join personalInfo in _context.PersonalInfos on user.PersonalInfoId equals personalInfo.Id
                    join speciality in _context.DoctorSpecialities on doctor.SpecialityId equals speciality.Id
                    select new { doctor, user, personalInfo, speciality };

        if (!string.IsNullOrEmpty(model.Name))
            query = query.Where(x => x.personalInfo.Name.Contains(model.Name));
        if (!string.IsNullOrEmpty(model.Surname))
            query = query.Where(x => x.personalInfo.Surname.Contains(model.Surname));
        if (!string.IsNullOrEmpty(model.Patronymic))
            query = query.Where(x => x.personalInfo.Patronymic.Contains(model.Patronymic));
        if (model.SpecialityId.HasValue)
            query = query.Where(x => x.doctor.SpecialityId == model.SpecialityId.Value);

        var results = await query.Select(x => new DoctorSearchResult
        {
            UserId = x.user.Id,
            PersonalInfoId = x.personalInfo.Id,
            SpecialityId = x.speciality.Id,
            SpecialityName = x.speciality.Name
        }).ToListAsync();

        model.Results = results;
        model.PersonalInfos = await _context.PersonalInfos
            .Where(p => results.Select(r => r.PersonalInfoId).Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p);
        model.Specialities = await _context.DoctorSpecialities.ToListAsync();

        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> EditDoctor(int id)
    {
        try
        {
            var (doctor, _, personalInfo) = await GetDoctorDataAsync(id);
            var model = new EditDoctorViewModel
            {
                UserId = doctor.Id,
                Surname = personalInfo.Surname,
                Name = personalInfo.Name,
                Patronymic = personalInfo.Patronymic,
                SpecialityId = doctor.SpecialityId,
                Specialities = await _context.DoctorSpecialities.ToListAsync()
            };

            return View(model);
        }
        catch (DoctorNotFoundException e)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDoctor(int id, EditDoctorViewModel model)
    {
        if (id != model.UserId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            model.Specialities = await _context.DoctorSpecialities.ToListAsync();
            return View(model);
        }

        try
        {
            var (doctor, user, personalInfo) = await GetDoctorDataAsync(model.UserId);
            personalInfo.Surname = model.Surname;
            personalInfo.Name = model.Name;
            personalInfo.Patronymic = model.Patronymic;
            doctor.SpecialityId = model.SpecialityId;
            await _context.SaveChangesAsync();
            return RedirectToAction("SearchDoctors");
        }
        catch (DoctorNotFoundException e)
        {
            return NotFound();
        }
    }
    
    private async Task<(Doctor doctor, User user, PersonalInfo info)> GetDoctorDataAsync(int id)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == id);
        if (doctor is null)
        {
            throw new DoctorNotFoundException("Doctor not found");
        }
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == doctor.Id);
        if (user is null)
        {
            throw new DoctorNotFoundException("Doctor not found");
        }
        var personalInfo = await _context.PersonalInfos.FirstOrDefaultAsync(p => p.Id == user.PersonalInfoId);
        if (personalInfo is null)
        {
            throw new DoctorNotFoundException("Doctor not found");
        }
        return (doctor, user, personalInfo);
    }
}