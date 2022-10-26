using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using findox.Domain.Interfaces.Service;
using Microsoft.IdentityModel.Tokens;

namespace findox.Service.Services
{
    public class TokenService : ITokenService
    {
        private string _key;

        public TokenService()
        {
            _key = $"{Environment.GetEnvironmentVariable("STORAGE_CRYPTO_KEY")}";
        }

        public string Encode(long id, string role)
        {
            var claims = new List<Claim>()
            {
                new Claim("id", id.ToString()),
                new Claim("role", role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtTokenDescriptor = new JwtSecurityToken(
                issuer: "findox",
                audience: "findox",
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                claims: claims,
                signingCredentials: signingCredentials
            );
            
            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtTokenDescriptor);
            return jwt;
        }
    }
}