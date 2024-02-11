using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Identity;

namespace AccountsProtector.AccountsProtector.Core.Domain.UnitOfWorkContracts
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Account> Accounts { get; }
        public UserManager<User> Users { get; }
        public IRepository<AccountAttribute> AccountAttributes { get; }
        public IRepository<Platform> Platforms { get; }
        public IRepository<OTP> OTPs { get; }
        public SignInManager<User> SignInManager { get; }
        Task SaveAsync();
    }
}
