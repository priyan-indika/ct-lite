using AutoMapper;
using Microsoft.Extensions.Logging;
using PractTest.Application.DTOs;
using PractTest.Domain.Entities;

namespace PractTest.Application
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.CreateMap<Customer, CustomerDto>().ReverseMap();
            configExpression.CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            configExpression.CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
            configExpression.CreateMap<CustomerDto, UpdateCustomerDto>().ReverseMap();
            configExpression.CreateMap<Order, OrderDto>().ReverseMap();
            configExpression.CreateMap<Order, CreateOrderDto>().ReverseMap();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var mappingConfig = new MapperConfiguration(configExpression, loggerFactory);
            return mappingConfig;
        }
    }
}
