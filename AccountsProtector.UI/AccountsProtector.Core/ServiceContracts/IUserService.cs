using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(User user, string password);
        Task<bool> LoginAsync(string email, string password);
        Task<User>? GetUserByEmailAsync(string email);
        Task<bool> UpdatePasswordAsync(string oldPassword, string newPassword, string email);
        Task<bool> UpdatePasswordAsync(string newPassword, string email);
    }
}