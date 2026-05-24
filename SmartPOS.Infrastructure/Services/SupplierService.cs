using Microsoft.EntityFrameworkCore;
using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Domain.Entities;
using SmartPOS.Infrastructure.Data;

namespace SmartPOS.Infrastructure.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupplierDto>> GetAllAsync()
        {
            return await _context.Suppliers
                .Select(s => new SupplierDto
                {
                    SupplierId = s.SupplierId,
                    SupplierName = s.SupplierName,
                    Phone = s.Phone,
                    Email = s.Email,
                    Address = s.Address
                })
                .ToListAsync();
        }

        public async Task CreateAsync(SupplierDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SupplierName))
                throw new Exception("Supplier name is required");

            var supplier = new Supplier
            {
                SupplierName = dto.SupplierName,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address
            };

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SupplierDto dto)
        {
            var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);

            if (supplier == null)
                throw new Exception("Supplier not found");

            supplier.SupplierName = dto.SupplierName;
            supplier.Phone = dto.Phone;
            supplier.Email = dto.Email;
            supplier.Address = dto.Address;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                throw new Exception("Supplier not found");

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }
}