using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<IdentityResult> Register(User user, string password);
        Task<bool> Login(string email, string password);
        Task<User>? GetUserByEmail(string email);
        Task<bool> UpdatePassword(string oldPassword, string newPassword, string email);
        Task<bool> UpdatePassword(string newPassword, string email);
    }
}