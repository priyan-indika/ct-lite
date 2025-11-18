using PractTest.Application.DTOs;

namespace PractTest.Application.Interfaces.Orders
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto);
    }
}
