using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.Domain.UnitOfWorkContracts;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;

namespace AccountsProtector.AccountsProtector.Core.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PlatformService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddPlatformAsync(DtoAddPlatformRequest request, string userEmail)
        {
            User? user = await _unitOfWork.Users.FindByEmailAsync(userEmail);
            if (user != null)
            {
                Platform platform = new Platform
                {
                    UserId = user.Id,
                    PlatformName = request.PlatformName,
                    IconColor = request.IconColor,
                };
                await _unitOfWork.Platforms.InsertAsync(platform);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<ICollection<Platform>> GetAllPlatforms(string userEmail)
        {
            User? user = await _unitOfWork.Users.FindByEmailAsync(userEmail);
            IEnumerable<Platform> platforms = await _unitOfWork.Platforms.SelectListByMatchAsync(p => p.UserId == user!.Id, new List<string>{"Accounts"});
            return platforms.ToList();
        }

        public Task<Platform> GetAllPlatformsWithAccounts(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<Platform> GetPlatformByIdWithAccounts(string platformId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePlatformAsync(int id, string userEmail)
        {
            User user = (await _unitOfWork.Users.FindByEmailAsync(userEmail))!;
            Platform platform = await _unitOfWork.Platforms.GetByIdAsync(id);
            if (platform != null && platform.UserId == user.Id)
            {
                await _unitOfWork.Platforms.DeleteAsync(platform);
                await _unitOfWork.SaveAsync();
                return true;
            }
            return false;
        }
    }
}
