using AccountsProtector.AccountsProtector.Core.Domain.Entities;

namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IPlatformService
    {
        ICollection<Account> GetAccounts(Guid platformId);
        bool AddAccount(Account account);
    }
}
