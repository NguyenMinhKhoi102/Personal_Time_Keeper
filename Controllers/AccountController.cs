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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index(Account account, string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            if (IsValidUser(account.Email, account.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Email)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Đăng nhập không hợp lệ.");
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

    private static bool IsValidUser(string email, string password)
    {
        return email == "test" && password == "password";
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
