using Microsoft.EntityFrameworkCore;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Domain.Entities;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Role = u.Role
                })
                .ToListAsync();
        }

        public async Task CreateAsync(CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new Exception("Username is required");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("Password is required");

            var exists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username);

            if (exists)
                throw new Exception("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new Exception("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}