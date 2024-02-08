using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.UnitOfWork;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace ServicesLayer.UserService
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
        public async Task<bool> Login(User user, string password)
        {
            SignInResult result = await _dp.SignInManager.PasswordSignInAsync(user.Email, password, isPersistent:true, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task<User>? GetUserByEmail(string email)
        {
            return await _dp.Users.FindByEmailAsync(email);
        }

        public async Task<bool> IsEmailIsAlreadyRegistered(string email)
        {
            return await _dp.Users.FindByEmailAsync(email) == null;
        }
    }
}