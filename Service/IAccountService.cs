using MyProject.Models;
using MyProject.Payload.Request;

namespace MyProject.Service
{
    public interface IAccountService
    {
        Task<Account> Register(RegisterRequest rq);
        Task<Account?> Login(LoginRequest rq);
        Task<Account> GetAccountById(int id);
        Task<bool> EditAccount(int id, AccountEditRequest rq);

        Task<bool> EmailExists(string email);
        Task<bool> PhoneExists(string phone);
    }
}

