using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartPOS.Application.Interfaces;
using SmartPOS.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartPOS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                throw new Exception("Invalid credentials");

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isValid)
                throw new Exception("Invalid credentials");

            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("JWT key is missing. Configure Jwt:Key using user-secrets or environment variables.");

            if (string.IsNullOrWhiteSpace(jwtIssuer))
                throw new InvalidOperationException("JWT issuer is missing. Configure Jwt:Issuer.");

            if (string.IsNullOrWhiteSpace(jwtAudience))
                throw new InvalidOperationException("JWT audience is missing. Configure Jwt:Audience.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task ChangePasswordAsync(string username, string oldPassword, string newPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                throw new Exception("User not found");

            bool isValid = BCrypt.Net.BCrypt.Verify(oldPassword, user.Password);

            if (!isValid)
                throw new Exception("Old password is incorrect");

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 4)
                throw new Exception("New password must be at least 4 characters");

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync();
        }
    }
}
