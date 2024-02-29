using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.Domain.UnitOfWorkContracts;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;

namespace AccountsProtector.AccountsProtector.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> CreateAccountAsync(DtoCreateAccountRequest request, string userId)
        {
            Platform platform = await _unitOfWork.Platforms.GetByIdAsync((int)request.PlatformId!);
            if (platform != null && platform.UserId.ToString() == userId)
            {
                Account account = new Account
                {
                    AccountName = request.AccountName,
                    PlatformId = (int)request.PlatformId!
                };
                await _unitOfWork.Accounts.InsertAsync(account);
                account.AccountAttributes = request.AccountFields.Select(field
                    => new AccountAttribute
                    {
                        AccountId = account.Id,
                        Name = field.Key,
                        Value = field.Value
                    }).ToList();

                await _unitOfWork.SaveAsync();
                return account.Id;
            }
            return -1;
        }

        public async Task<DtoGetAccountsByPlatformIdResponse?> GetAccountsByPlatformIdAsync(int? requestPlatformId, string userId)
        {
            Platform platform = await _unitOfWork.Platforms.GetByIdAsync((int)requestPlatformId!, "Accounts");
            if (platform != null && platform.UserId.ToString() == userId)
            {
                IEnumerable<Account> accounts =
                    await _unitOfWork.Accounts.SelectListByMatchAsync(a => a.PlatformId == requestPlatformId,
                        "AccountAttributes");
                return new DtoGetAccountsByPlatformIdResponse
                {
                    Accounts = accounts.Select(a => new DtoAccount
                    {
                        AccountId = a.Id,
                        AccountName = a.AccountName,
                        PlatformId = a.PlatformId,
                        AccountFields = a.AccountAttributes.Select(aa
                                => new KeyValuePair<string, string>(aa.Name, aa.Value))
                            .ToDictionary(kv => kv.Key, kv => kv.Value)
                    }).ToList()
                };
            }
            return null;
        }
    }
}
