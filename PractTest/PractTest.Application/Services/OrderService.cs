using AutoMapper;
using PractTest.Application.DTOs;
using PractTest.Application.Interfaces.Customers;
using PractTest.Application.Interfaces.Orders;
using PractTest.Domain.Entities;

namespace PractTest.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = _mapper.Map<Order>(createOrderDto);
            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            if (createdOrder == null) return null;

            var customer = await _customerRepository.GetCustomerByIdAsync(createdOrder.CustomerId);
            if (customer is null) return null;

            customer.LoyaltyPoints = customer.LoyaltyPoints + (int)(createdOrder.OrderTotal / 10);
            var updatedCustomer = await _customerRepository.UpdateCustomerAsync(customer);
            if (updatedCustomer is null) return null;

            var orderDto = _mapper.Map<OrderDto>(createdOrder);
            return orderDto;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var allOrders = await _orderRepository.GetAllOrdersAsync();
            var allOrderDtos = _mapper.Map<IEnumerable<OrderDto>>(allOrders);
            return allOrderDtos;
        }
    }
}
