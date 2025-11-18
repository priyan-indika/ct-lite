using Microsoft.EntityFrameworkCore;
using PractTest.Application.Interfaces.Orders;
using PractTest.Domain.Entities;

namespace PractTest.Infrastructure
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var allOrders = await _dbContext.Orders.ToListAsync();
            return allOrders;
        }
    }
}
