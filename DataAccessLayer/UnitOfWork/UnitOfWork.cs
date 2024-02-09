using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DataBase;
using DataLayer.Models;
using DataLayer.Repository;
using ElBashaStoreRestAPI.Repository;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dp;
        public IRepository<Account> Accounts { get; }
        public UserManager<User> Users { get; private set; }
        public IRepository<AccountAttribute> AccountAttributes { get; }
        public IRepository<Platform> Platforms { get; }
        public SignInManager<User> SignInManager { get; }
        public UnitOfWork(AppDbContext dp, UserManager<User> users, SignInManager<User> signInManager)
        {
            _dp = dp;
            Accounts = new Repository<Account>(_dp);
            Users = users;
            SignInManager = signInManager;
            AccountAttributes = new Repository<AccountAttribute>(_dp);
            Platforms = new Repository<Platform>(_dp);
        }
        public async Task SaveAsync()
        {
            await _dp.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dp.Dispose();
        }
    }
}
