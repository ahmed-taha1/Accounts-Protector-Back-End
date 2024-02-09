using AccountsProtector.Core.Domain.Entities;

namespace AccountsProtector.Core.ServiceContracts
{
    public interface IJwtService
    {
        public string GenerateToken(User user);
        public bool ValidateToken(string token);
        public string? GetEmailFromToken(string token);
    }
}