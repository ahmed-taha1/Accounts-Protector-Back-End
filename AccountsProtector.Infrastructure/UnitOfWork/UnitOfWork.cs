using AccountsProtector.Core.Domain.Entities;
using AccountsProtector.Infrastructure.Repositories;
using DataAccessLayer.UnitOfWork;
using DataLayer.Repository;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext.AppDbContext _dp;
        public IRepository<Account> Accounts { get; }
        public UserManager<User> Users { get; private set; }
        public IRepository<AccountAttribute> AccountAttributes { get; }
        public IRepository<Platform> Platforms { get; }
        public SignInManager<User> SignInManager { get; }
        public UnitOfWork(AppDbContext.AppDbContext dp, UserManager<User> users, SignInManager<User> signInManager)
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
