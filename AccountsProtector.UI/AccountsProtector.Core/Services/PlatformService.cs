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
    }
}
