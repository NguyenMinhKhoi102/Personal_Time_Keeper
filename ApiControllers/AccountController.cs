using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Controllers;
using MyProject.Models;
using MyProject.Payload.Request;
using MyProject.Payload.Response;
using MyProject.Service;


namespace MyProject.ApiControllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // POST api/values
        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] RegisterRequest rq)
        {
            try
            {
                if (await _accountService.EmailExists(rq.Email))
                    return BadRequest(new MessageResponse("Email already exists"));

                if (rq.Phone != null && await _accountService.PhoneExists(rq.Phone))
                    return BadRequest(new MessageResponse("Phone already exists"));

                var account = await _accountService.Register(rq);

                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] LoginRequest rq)
        {
            try
            {
                var account = await _accountService.Login(rq);
                if (account != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                        new Claim(ClaimTypes.Name, account.FullName),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Ok(new MessageResponse("Login successfully"));
                }
                return BadRequest(new MessageResponse("Login fail ! Because email or password were incorrect"));
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse(ex.Message));
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Post([FromBody] AccountEditRequest rq)
        {
            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (accountIdClaim == null)
                return Unauthorized(new MessageResponse("User not logged in"));

            int accountId = int.Parse(accountIdClaim.Value);

            var result = await _accountService.EditAccount(accountId, rq);

            if (result)
                return Ok(new MessageResponse("Updated successfully"));
            return BadRequest(new MessageResponse("Update failed"));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Post()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("info")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (accountIdClaim == null)
                    return Unauthorized(new MessageResponse("User not logged in"));

                int accountId = int.Parse(accountIdClaim.Value);
                var account = await _accountService.GetAccountById(accountId);
                if (account == null)
                    return NotFound(new MessageResponse("Account not found"));
                var accountResponse = new AccountResponse()
                {
                    Email = account.Email,
                    FullName = account.FullName,
                    Phone = account.Phone
                };
                return Ok(accountResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse(ex.Message));
            }

        }



    }
}

