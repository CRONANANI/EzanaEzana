using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EzanaEzana.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EzanaEzana.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
        ClaimsPrincipal? ValidateToken(string token);
        string? GetUserIdFromToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? "your-super-secret-key-with-at-least-32-characters");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()),
                new Claim("FirstName", user.FirstName ?? ""),
                new Claim("LastName", user.LastName ?? ""),
                new Claim("PublicUsername", user.PublicUsername ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token expires in 7 days
                Issuer = _configuration["Jwt:Issuer"] ?? "Ezana",
                Audience = _configuration["Jwt:Audience"] ?? "EzanaUsers",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? "your-super-secret-key-with-at-least-32-characters");

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"] ?? "Ezana",
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"] ?? "EzanaUsers",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string? GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
