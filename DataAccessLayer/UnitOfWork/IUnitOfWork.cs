using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using DataLayer.Repository;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Account> Accounts { get; }
        public UserManager<User> Users { get; }
        public IRepository<AccountAttribute> AccountAttributes { get; }
        public IRepository<Platform> Platforms { get; }
        public SignInManager<User> SignInManager { get; }
        Task SaveAsync();
    }
}
