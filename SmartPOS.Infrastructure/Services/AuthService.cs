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

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
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