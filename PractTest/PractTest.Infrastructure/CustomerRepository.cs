using Microsoft.EntityFrameworkCore;
using PractTest.Application.Interfaces.Customers;
using PractTest.Domain.Entities;

namespace PractTest.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = await _dbContext.Customers.ToListAsync();
            return customers;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);
            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            await _dbContext.SaveChangesAsync();
            return customer;
        }
    }
}
