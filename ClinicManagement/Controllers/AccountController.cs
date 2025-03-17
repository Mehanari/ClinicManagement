using System.Security.Claims;
using ClinicManagement.Context;
using ClinicManagement.Models;
using ClinicManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Controllers;

public class AccountController : Controller
{
    private readonly MyDbContext _context;

    public AccountController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Login == model.Email && u.Password == model.Password);

            if (user != null)
            {
                var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.Name ?? "Unknown";
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Profile");
            }
            ModelState.AddModelError("", "Невірний логін або пароль");
        }
        return View(model);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Profile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var user = _context.Users
            .FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return NotFound();
        }
        
        var personalInfo = _context.PersonalInfos.FirstOrDefault(p => p.Id == user.PersonalInfoId);
        var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
        if(personalInfo == null)
        {
            return NotFound();
        }
        var gender = _context.Genders.FirstOrDefault(g => g.Id == personalInfo.GenderId);
        personalInfo.Gender = gender;
        var viewModel = new ProfileViewModel
        {
            Login = user.Login,
            Role = role is null ? "Undefined" : role.Name,
            PersonalInfo = personalInfo
        };


        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}