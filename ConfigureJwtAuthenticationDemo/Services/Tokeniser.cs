using ConfigureJwtAuthenticationDemo.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConfigureJwtAuthenticationDemo.Services
{
    public class Tokeniser
    {
        private readonly IConfiguration _configuration;
        public Tokeniser(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var getKey = Environment.GetEnvironmentVariable("KEY");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(getKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
             
            var token = new JwtSecurityToken(
                 issuer: _configuration["Jwt:Issuer"],
                 audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
