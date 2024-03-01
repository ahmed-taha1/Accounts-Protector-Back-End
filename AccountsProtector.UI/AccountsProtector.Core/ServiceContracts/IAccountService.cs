using AccountsProtector.AccountsProtector.Core.DTO;

namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IAccountService
    {
        Task<int> CreateAccountAsync(DtoCreateAccountRequest request, string userId);
        Task<DtoGetAccountsByPlatformIdResponse?> GetAccountsByPlatformIdAsync(int? requestPlatformId, string userId);
        Task<DtoAccount?> GetAccountByIdAsync(int accountId, string userId);
        Task<bool> DeleteAccountAsync(int accountId, string userId);
        Task<bool> UpdateAccountAsync(DtoUpdateAccountRequest request, string userId);
    }
}