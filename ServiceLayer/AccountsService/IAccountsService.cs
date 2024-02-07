using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.AccountsService
{
    public interface IAccountsService
    {
        public Task<bool> Register();
    }
}
