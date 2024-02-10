using AccountsProtector.Core.Domain.Entities;
using AccountsProtector.Core.ServiceContracts;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _dp;

        public UserService(IUnitOfWork unitOfWork)
        {
            _dp = unitOfWork;
        }
        public async Task<IdentityResult> Register(User user, string password)
        {
            IdentityResult result = await _dp.Users.CreateAsync(user, password);
            await _dp.SaveAsync();
            return result;
        }
        public async Task<bool> Login(string email, string password)
        {
            SignInResult result = await _dp.SignInManager.PasswordSignInAsync(email, password, isPersistent:true, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<User>? GetUserByEmail(string email)
        {
            return await _dp.Users.FindByEmailAsync(email);
        }

        public async Task<bool> UpdatePassword(string oldPassword, string newPassword, string email)
        {
            User user = await _dp.Users.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var result = await _dp.Users.ChangePasswordAsync(user, oldPassword, newPassword);
            await _dp.SaveAsync();
            return result.Succeeded;
        }

        public async Task<bool> UpdatePassword(string newPassword, string email)
        {
            var user = await _dp.Users.FindByEmailAsync(email);

            if (user == null)
            {
                // User not found
                return false;
            }

            var token = await _dp.Users.GeneratePasswordResetTokenAsync(user);
            var result = await _dp.Users.ResetPasswordAsync(user, token, newPassword);
            await _dp.SaveAsync();
            return result.Succeeded;
        }
    }
}