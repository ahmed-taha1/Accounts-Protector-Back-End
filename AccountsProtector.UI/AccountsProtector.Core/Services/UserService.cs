using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.Domain.UnitOfWorkContracts;
using AccountsProtector.AccountsProtector.Core.Helpers;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.AccountsProtector.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _dp;

        public UserService(IUnitOfWork unitOfWork)
        {
            _dp = unitOfWork;
        }
        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            IdentityResult result = await _dp.Users.CreateAsync(user, password);
            user.Platforms = new List<Platform>
            {
                new Platform
                {
                    UserId = user.Id,
                    PlatformName = "Facebook",
                    IconColor = "ff38569E",
                },
                new Platform
                {
                    UserId = user.Id,
                    PlatformName = "Instagram",
                    IconColor = "ffe14a80",
                },
                new Platform
                {
                    UserId = user.Id,
                    PlatformName = "Google",
                    IconColor = "fffbbe0d",
                }
            };
            await _dp.SaveAsync();
            return result;
        }
        public async Task<bool> LoginAsync(string email, string password)
        {
            SignInResult result = await _dp.SignInManager.PasswordSignInAsync(email, password, isPersistent:true, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dp.Users.FindByEmailAsync(email);
        }

        public async Task<bool> UpdatePasswordAsync(string oldPassword, string newPassword, string email)
        {
            User? user = await _dp.Users.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var result = await _dp.Users.ChangePasswordAsync(user, oldPassword, newPassword);
            await _dp.SaveAsync();
            return result.Succeeded;
        }

        public async Task<bool> UpdatePasswordAsync(string newPassword, string email)
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

        public async Task<bool> SetPinAsync(string pin, string userEmail)
        {
            User? user = await _dp.Users.FindByEmailAsync(userEmail);
            if (user != null && user.PinHash == null)
            {
                user.PinHash = HashHelper.Hash(pin);
                await _dp.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CheckPinAsync(string pin, string userEmail)
        {
            User? user = await _dp.Users.FindByEmailAsync(userEmail);
            if (user != null && user.PinHash == null)
            {
                String hashedPin = HashHelper.Hash(pin);
                return user.PinHash == hashedPin;
            }
            return false;
        }
    }
}