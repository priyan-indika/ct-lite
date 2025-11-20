using AutoMapper;
using PractTest.Application.DTOs;
using PractTest.Application.Interfaces.Customers;
using PractTest.Application.Mappings;
using PractTest.Domain.Entities;

namespace PractTest.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomerAsync()
        {
            var customerList = await _customerRepository.GetAllCustomersAsync();
            if (customerList is null) return [];

            //var allCustomerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customerList);
            var customerDtoList = customerList.ToList().ToDtos();
            return customerDtoList;
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer is null) return null;

            //var customerDto = _mapper.Map<CustomerDto>(customer);
            var customerDto = customer.ToDto();
            return customerDto;
        }

        public async Task<CustomerDto?> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createCustomerDto);
            customer.Username = createCustomerDto.Name.ToLower();
            customer.Password = createCustomerDto.Name.ToUpper();
            customer.CreatedBy = 1000;

            var customerCreated = await _customerRepository.CreateCustomerAsync(customer);
            if (customerCreated is null) return null;

            var customerDto = _mapper.Map<CustomerDto>(customerCreated);
            return customerDto;
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
            if (existingCustomer is null) return null;

            existingCustomer.Name = customerDto.Name;
            existingCustomer.Email = customerDto.Email;
            existingCustomer.LoyaltyPoints = customerDto.LoyaltyPoints;

            var updatedCustomer = await _customerRepository.UpdateCustomerAsync(existingCustomer);
            if (updatedCustomer is null) return null;

            var updatedCustomerDto = _mapper.Map<CustomerDto>(updatedCustomer);
            return updatedCustomerDto;
        }

        public async Task<CustomerDto?> UpdateLoyaltyPointsAsync(int id, int loyalty)
        {
            var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
            if (existingCustomer is null) return null;

            existingCustomer.LoyaltyPoints = loyalty;

            var updatedCustomer = await _customerRepository.UpdateCustomerAsync(existingCustomer);
            if (updatedCustomer is null) return null;

            var customerDto = _mapper.Map<CustomerDto>(updatedCustomer);
            return customerDto;
        }
    }
}
