using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountsProtector.AccountsProtector.Infrastructure.AppDbContext
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<AccountAttribute> AccountAttributes { get; set; }
        public DbSet<OTP> OTPs { get; set; }
    }
}