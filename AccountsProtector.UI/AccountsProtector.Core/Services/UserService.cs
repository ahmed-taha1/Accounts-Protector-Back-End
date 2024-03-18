using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.Domain.UnitOfWorkContracts;
using AccountsProtector.AccountsProtector.Core.Helpers;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using Azure.Core;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.AccountsProtector.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            IdentityResult result = await _unitOfWork.Users.CreateAsync(user, password);
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
            await _unitOfWork.SaveAsync();
            return result;
        }
        public async Task<bool> LoginAsync(string email, string password)
        {
            SignInResult result = await _unitOfWork.SignInManager.PasswordSignInAsync(email, password, isPersistent:true, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.Users.FindByEmailAsync(email);
        }

        public async Task<bool> UpdatePasswordAsync(string oldPassword, string newPassword, string email)
        {
            User? user = await _unitOfWork.Users.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var result = await _unitOfWork.Users.ChangePasswordAsync(user, oldPassword, newPassword);
            await _unitOfWork.SaveAsync();
            return result.Succeeded;
        }

        public async Task<bool> UpdatePasswordAsync(string newPassword, string email)
        {
            var user = await _unitOfWork.Users.FindByEmailAsync(email);

            if (user == null)
            {
                // User not found
                return false;
            }

            var token = await _unitOfWork.Users.GeneratePasswordResetTokenAsync(user);
            var result = await _unitOfWork.Users.ResetPasswordAsync(user, token, newPassword);
            await _unitOfWork.SaveAsync();
            return result.Succeeded;
        }

        public async Task<bool> SetPinAsync(string pinHash, string userEmail)
        {
            var user = await _unitOfWork.Users.FindByEmailAsync(userEmail);
            if (user != null && !string.IsNullOrEmpty(user.PinHash))
            {
                user.PinHash = pinHash;
                return true;
            }
            return false;
        }

        public async Task<bool> CheckPinAsync(string pinHash, string userEmail)
        {
            User? user = await _unitOfWork.Users.FindByEmailAsync(userEmail);
            if (user != null && user.PinHash != null)
            {
                return user.PinHash == pinHash;
            }
            return false;
        }

        public async Task<bool> RemoveAccountAsync(string password, string email)
        {
            User? user = await _unitOfWork.Users.FindByEmailAsync(email);
            if (user != null && await _unitOfWork.Users.CheckPasswordAsync(user, password))
            {
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }
    }
}