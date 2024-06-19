using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using MyProject.AppData;
using MyProject.Models;
using MyProject.Payload.Request;

namespace MyProject.Service
{
    public class AccountService : IAccountService
    {
        private readonly AppDBContext _context;

        public AccountService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Account> Register(RegisterRequest rq)
        {
            var account = new Account
            {
                Email = rq.Email,
                FullName = rq.FullName,
                Password = HashPassword(rq.Password),
                Phone = rq.Phone
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account?> Login(LoginRequest rq)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == rq.Email);
            if (account != null && BCrypt.Net.BCrypt.Verify(rq.Password, account.Password))
            {
                return account;
            }
            return null;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Accounts.AnyAsync(u => u.Email == email);

        }

        public async Task<bool> PhoneExists(string phone)
        {
            return await _context.Accounts.AnyAsync(u => u.Phone == phone);

        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<Account> GetAccountById(int id)
        {
            return await _context.Accounts.FirstAsync(a => a.Id == id);
        }

        public async Task<bool> EditAccount(int id, AccountEditRequest rq)
        {
            try
            {
                var account = await GetAccountById(id);


                account.FullName = rq.FullName;
                account.Phone = rq.Phone;
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}

