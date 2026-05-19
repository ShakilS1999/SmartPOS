using SmartPOS.Application.DTOs;
using SmartPOS.Application.Interfaces;
using SmartPOS.Domain.Entities;

namespace SmartPOS.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _repo.GetAllAsync();

            return customers.Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Phone = c.Phone
            }).ToList();
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);

            if (c == null)
                return null;

            return new CustomerDto
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                Phone = c.Phone
            };
        }

        public async Task CreateAsync(CustomerDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CustomerName))
                throw new Exception("Customer name is required");

            var customer = new Customer
            {
                CustomerName = dto.CustomerName,
                Phone = dto.Phone
            };

            await _repo.AddAsync(customer);
        }

        public async Task UpdateAsync(CustomerDto dto)
        {
            var customer = await _repo.GetByIdAsync(dto.CustomerId);

            if (customer == null)
                throw new Exception("Customer not found");

            customer.CustomerName = dto.CustomerName;
            customer.Phone = dto.Phone;

            await _repo.UpdateAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}