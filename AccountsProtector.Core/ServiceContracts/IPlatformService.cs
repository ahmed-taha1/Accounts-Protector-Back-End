using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountsProtector.Core.Domain.Entities;

namespace AccountsProtector.Core.ServiceContracts
{
    public interface IPlatformService
    {
        ICollection<Account> GetAccounts(Guid platformId);
        bool AddAccount(Account account);
    }
}
