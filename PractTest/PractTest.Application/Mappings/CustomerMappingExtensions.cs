using PractTest.Application.DTOs;
using PractTest.Domain.Entities;

namespace PractTest.Application.Mappings
{
    public static class CustomerMappingExtensions
    {
        public static Customer ToEntity(this CustomerDto dto)
        {
            return new Customer
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                LoyaltyPoints = dto.LoyaltyPoints
            };
        }

        public static CustomerDto ToDto(this Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                LoyaltyPoints = customer.LoyaltyPoints
            };
        }

        public static IEnumerable<Customer> ToEntities(this IEnumerable<CustomerDto> dtos)
            => dtos.Select(dto => dto.ToEntity()).ToList();

        public static IEnumerable<CustomerDto> ToDtos(this IEnumerable<Customer> entities)
            => entities.Select(entity => entity.ToDto()).ToList();
    }
}
