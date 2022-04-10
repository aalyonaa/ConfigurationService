using Marvelous.Contracts.Enums;
using MarvelousConfigs.BLL.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MarvelousConfigs.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;
        }

        public string GetToken(string email, string pass)
        {
            _logger.LogInformation("User login and password verification");
            if (email == "string" && pass == "string") // как только identity сервис будет готов, нужно будет изменить!
            {
                var claims = new List<Claim> {
                new Claim(ClaimTypes.Role, Role.Admin.ToString() )
                };

                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.Issuer,
                        audience: AuthOptions.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)), // время дейCтвия 30 минут
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));
                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            else
            {
                throw new Exception();
            }
        }

    }
}
