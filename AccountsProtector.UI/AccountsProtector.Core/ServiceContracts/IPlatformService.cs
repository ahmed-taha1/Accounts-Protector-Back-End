using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.DTO;

namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IPlatformService
    {
        Task<bool> AddPlatformAsync(Platform request, string userEmail);
        Task<ICollection<Platform>> GetAllPlatforms(string userEmail);
        Task<Platform> GetPlatformById(int platformId);
        Task<bool> DeletePlatformAsync(int id, string userEmail);
        Task<bool> UpdatePlatformAsync(Platform platform, string userEmail);
    }
}
