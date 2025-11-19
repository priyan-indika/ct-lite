using PractTest.Application.DTOs;

namespace PractTest.Application.Interfaces.Customers
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomerAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int id);
        Task<CustomerDto?> CreateCustomerAsync(CreateCustomerDto customerDto);
        Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto customerDto);
    }
}
