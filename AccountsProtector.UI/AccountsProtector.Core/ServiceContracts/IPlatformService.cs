using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;

namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IPlatformService
    {
        Task<bool> AddPlatformAsync(DtoAddPlatformRequest request, string userEmail);
        Task<ICollection<Platform>> GetAllPlatforms(string userEmail);
        Task<Platform> GetAllPlatformsWithAccounts(string userEmail);
        Task<Platform> GetPlatformByIdWithAccounts(string platformId);
        Task<bool> DeletePlatformAsync(int id, string userEmail);
    }
}
