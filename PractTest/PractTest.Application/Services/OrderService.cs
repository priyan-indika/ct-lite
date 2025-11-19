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
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICustomerService customerService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _customerService = customerService;
            _mapper = mapper;
        }

        public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = _mapper.Map<Order>(createOrderDto);
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            if (createdOrder == null)
            {
                return null;
            }

            var customer = await _customerService.GetCustomerByIdAsync(createdOrder.CustomerId);
            var updateCustomerDto = _mapper.Map<UpdateCustomerDto>(customer);
            updateCustomerDto.LoyaltyPoints = updateCustomerDto.LoyaltyPoints + (int)(createdOrder.OrderTotal / 10);
            await _customerService.UpdateCustomerAsync(customer.Id, updateCustomerDto);

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
