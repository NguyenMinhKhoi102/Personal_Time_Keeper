using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.AppData;
using MyProject.Models;


namespace MyProject.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDBContext _context;

    public AccountController(ILogger<HomeController> logger, AppDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        ViewData["IsLoginPage"] = true;
        return View();
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        ViewData["IsLoginPage"] = true;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Register(Account account)
    {
        if (ModelState.IsValid)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return RedirectToAction("Index", "Account");
        }
        ViewData["IsLoginPage"] = true;
        return View(account);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private bool IsValidUser(string email, string password)
    {
        var account = _context.Accounts.FirstOrDefault(a => a.Email == email);
        if (account == null)
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(password, account.Password);
    }

    public IActionResult Edit()
    {
        return View();
    }

    public ILogger<HomeController> GetLogger()
    {
        return _logger;
    }

    public AppDBContext GetContext()
    {
        return _context;
    }
}
