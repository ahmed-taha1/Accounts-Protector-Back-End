using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsProtector.Core.ServiceContracts
{
    public interface IEmailService
    {
        public Task<bool> SendOTP(string email);
        public Task<bool> VerifyOTP(string email, int otp);
    }
}