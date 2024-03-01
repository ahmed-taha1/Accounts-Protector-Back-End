using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.Domain.UnitOfWorkContracts;
using AccountsProtector.AccountsProtector.Core.DTO;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using AccountsProtector.AccountsProtector.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

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
            Platform? platform = await _unitOfWork.Platforms.GetByIdAsync((int)request.PlatformId!);
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
                        Name = EncryptionHelper.Encrypt(field.Key),
                        Value = EncryptionHelper.Encrypt(field.Value)
                    }).ToList();

                await _unitOfWork.SaveAsync();
                return account.Id;
            }
            return -1;
        }

        public async Task<DtoGetAccountsByPlatformIdResponse?> GetAccountsByPlatformIdAsync(int? requestPlatformId, string userId)
        {
            Platform? platform = await _unitOfWork.Platforms.GetByIdAsync((int)requestPlatformId!, nameof(AppDbContext.Accounts));
            if (platform != null && platform.UserId.ToString() == userId)
            {
                IEnumerable<Account?>? accounts =
                    await _unitOfWork.Accounts.SelectListByMatchAsync(a => a.PlatformId == requestPlatformId,
                        nameof(AppDbContext.AccountAttributes));
                if (accounts != null)
                {
                    return new DtoGetAccountsByPlatformIdResponse
                    {
                        Accounts = accounts.Select(a => new DtoAccount
                        {
                            AccountId = a.Id,
                            AccountName = a.AccountName,
                            PlatformId = a.PlatformId,
                            AccountFields = a.AccountAttributes!.Select(aa
                                    => new KeyValuePair<string, string>(EncryptionHelper.Decrypt(aa.Name!),
                                        EncryptionHelper.Decrypt(aa.Value!)))
                                .ToDictionary(kv => kv.Key, kv => kv.Value)
                        }).ToList()
                    };
                }
            }
            return null;
        }

        public async Task<DtoAccount?> GetAccountByIdAsync(int accountId, string userId)
        {
            Account? account = await _unitOfWork.Accounts.GetByIdAsync(accountId, nameof(AppDbContext.AccountAttributes));
            if (account == null)
            {
                return null;
            }
            Platform? platform = await _unitOfWork.Platforms.GetByIdAsync(account!.PlatformId);
            if (platform != null && platform.UserId.ToString() == userId)
            {
                DtoAccount dtoAccount = new DtoAccount
                {
                    AccountId = account.Id,
                    AccountName = account.AccountName,
                    PlatformId = account.PlatformId,
                    AccountFields = account.AccountAttributes!.Select(aa
                            => new KeyValuePair<string, string>(EncryptionHelper.Decrypt(aa.Name!),
                                EncryptionHelper.Decrypt(aa.Value!)))
                        .ToDictionary(kv => kv.Key, kv => kv.Value)
                };
                return dtoAccount;
            }

            return null;
        }

        public async Task<bool> DeleteAccountAsync(int accountId, string userId)
        {
            Account? account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
            if (account != null)
            {
                Platform? platform = await _unitOfWork.Platforms.GetByIdAsync(account.PlatformId);
                if (platform!.UserId.ToString() == userId)
                {
                    await _unitOfWork.Accounts.DeleteAsync(account);
                    await _unitOfWork.SaveAsync();

                }
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAccountAsync(DtoUpdateAccountRequest request, string userId)
        {
            Account? account = await _unitOfWork.Accounts.GetByIdAsync((int)request.AccountId!, nameof(AppDbContext.AccountAttributes));
            if (account != null)
            {
                Platform? platform = await _unitOfWork.Platforms.GetByIdAsync(account.PlatformId);
                if (platform!.UserId.ToString() == userId)
                {
                    if (request.AccountName != null)
                    {
                        account.AccountName = request.AccountName;
                    }

                    if (request.AccountFields != null)
                    {
                        foreach (var field in request.AccountFields)
                        {
                            bool isFieldExist = false;
                            foreach (var accountAttribute in account.AccountAttributes!)
                            {
                                if (EncryptionHelper.Decrypt(accountAttribute.Name!) == field.Key)
                                {
                                    isFieldExist = true;
                                    accountAttribute.Value = EncryptionHelper.Encrypt(field.Value);
                                }
                            }

                            if (!isFieldExist)
                            {
                                account.AccountAttributes!.Add(new AccountAttribute
                                {
                                    AccountId = account.Id,
                                    Name = EncryptionHelper.Encrypt(field.Key),
                                    Value = EncryptionHelper.Encrypt(field.Value)
                                });
                            }
                        }
                    }
                    await _unitOfWork.SaveAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
