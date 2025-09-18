using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechMarketplace.API.Models.Users;

namespace TechMarketplace.API.Services
{
    public class JwtServices
    {
        private readonly IConfiguration _config;

        public JwtServices(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(int userId, string username, List<UserRole> role)
        {
            var JwtSettings = _config.GetSection("JwtSettings");
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["Key"]));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
             {
             new Claim(ClaimTypes.Name, username),
             new Claim(ClaimTypes.NameIdentifier, userId.ToString())
             };

            claims.AddRange(role.Select(x => new Claim(ClaimTypes.Role, x.ToString())));

            var token = new JwtSecurityToken(
                issuer: JwtSettings["Issuer"],
                audience: JwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(JwtSettings["ExpireInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
