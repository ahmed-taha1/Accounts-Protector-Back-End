using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ServicesLayer.UserService
{
    public interface IUserService
    {
        Task<IdentityResult> Register(User user, string password);
        Task<bool> Login(User user, string password);
        Task<User>? GetUserByEmail(string email);
        Task<bool> IsEmailIsAlreadyRegistered(string email);
    }
}
