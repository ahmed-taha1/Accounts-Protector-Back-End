using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;

namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IPlatformService
    {
        Task<bool> CreatePlatformAsync(Platform request, string userEmail);
        Task<ICollection<Platform?>?> GetAllPlatforms(string userId);
        Task<Platform?> GetPlatformByIdAsync(int platformId, string userId);
        Task<bool> DeletePlatformAsync(int id, string userId);
        Task<bool> UpdatePlatformAsync(Platform platform, string userEmail);
    }
}
