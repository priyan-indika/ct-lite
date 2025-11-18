using PractTest.Domain.Entities;

namespace PractTest.Application.Interfaces.Orders
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> CreateOrderAsync(Order order);
    }
}
