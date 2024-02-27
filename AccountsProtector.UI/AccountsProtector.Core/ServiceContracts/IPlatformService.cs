using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;

namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IPlatformService
    {
        Task<bool> AddPlatformAsync(DtoAddPlatformRequest request, string userEmail);
        // ICollection<Account> GetAccounts(Guid platformId);
        // bool AddAccount(Account account);
    }
}
