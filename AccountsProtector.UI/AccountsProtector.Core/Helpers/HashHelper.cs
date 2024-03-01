using System.Security.Cryptography;
using System.Text;

namespace AccountsProtector.AccountsProtector.Core.Helpers
{
    public static class HashHelper
    {
        public static string Hash(string text)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
