namespace AccountsProtector.AccountsProtector.Core.ServiceContracts
{
    public interface IEmailService
    {
        public Task<bool> SendOTP(string email);
        public Task<bool> VerifyOTP(string email, int otp);
    }
}