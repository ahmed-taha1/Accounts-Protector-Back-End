using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountsProtector.AccountsProtector.Core.Domain.Entities;
using AccountsProtector.AccountsProtector.Core.ServiceContracts;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AccountsProtector.AccountsProtector.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user, DateTime? customExpirationDate)
        {
            DateTime expirationDate = DateTime.UtcNow.AddDays(Convert.ToDouble(
                _configuration["JWT:EXPIRY_IN_DAYS"]));
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FirstName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()), // makes the token unreadable and casues an error
            };
            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            SigningCredentials creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: customExpirationDate ?? expirationDate,
                signingCredentials: creds
            );
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Issuer"],

                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateLifetime = true // Check token expiration
            };

            try
            {
                // Validate the token
                tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                return true; // Token is valid
            }
            catch (SecurityTokenException)
            {
                return false; // Token validation failed
            }
            catch (Exception)
            {
                return false; // Other exceptions
            }
        }

        public string? GetEmailFromToken(string token)
        {
            try
            {
                token = token.Split(' ').LastOrDefault()!;
                // Decode the token to retrieve its claims
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null)
                {
                    return null;
                }

                var email = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                return email;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}