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
            // if (await FindUserByEmail(user.Email) != null)
            // {
            //     throw new Exception("User email already exists");
            // }
            IdentityResult result = await _dp.Users.CreateAsync(user, password);
            await _dp.SaveAsync();
            return result;
        }

        public async Task<User>? FindUserByEmail(string email)
        {
            return await _dp.Users.FindByEmailAsync(email);
        }

        public async Task<bool> IsEmailIsAlreadyRegistered(string email)
        {
            return await _dp.Users.FindByEmailAsync(email) == null;
        }
    }
}