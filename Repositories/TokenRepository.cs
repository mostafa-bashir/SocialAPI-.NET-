using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Social.Repositories;
using SocialAPI.Models.Domains;

namespace SocialAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            // Read and validate the token
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new ArgumentException("Invalid JWT token");
            }

            // Extract the claims for Email, Name, and NameIdentifier
            var emailClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var nameClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var nameIdentifierClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier); // Use ClaimTypes.NameIdentifier

            // Create the user object and populate it with the claims
            var user = new User
            {
                Email = emailClaim?.Value ?? string.Empty,
                Name = nameClaim?.Value ?? string.Empty,
                Id = int.TryParse(nameIdentifierClaim?.Value, out int userId) ? userId : 0 // Safely parse the user ID
            };

            return user;
        }

    }
}
