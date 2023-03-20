using Kiwify.Core.Data;
using Kiwify.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kiwify.Core.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetAllOrdersByEmail(string buyerEmail);
        Task<Order?> GetOrderByOrderIdAndEmail(string orderId, string buyerEmail);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderAsync(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByEmail(string buyerEmail)
        {
            return await _context.Orders
                .Where(x => x.BuyerEmail.Equals(buyerEmail))
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByOrderIdAndEmail(string orderId, string buyerEmail)
        {
            return await _context.Orders.SingleOrDefaultAsync(x => x.OrderId == orderId && x.BuyerEmail == buyerEmail);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateOrderAsync(Order order)
        {
            var result = await GetOrderByOrderIdAndEmail(order.OrderId, order.BuyerEmail);
            if (result == null) return null;

            _context.Entry(result).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
