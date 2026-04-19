using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Services
{
    public interface IOrderService
    {
        Task<List<Order.API.Models.Order>> GetAllOrdersAsync();
        Task<Order.API.Models.Order> GetOrderByIdAsync(int id);
        Task<Order.API.Models.Order> CreateOrderAsync(Order.API.Models.Order order);
        Task<Order.API.Models.Order> UpdateOrderAsync(Order.API.Models.Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<List<Order.API.Models.Order>> GetPendingOrdersAsync();
        Task<List<Order.API.Models.Order>> GetOrdersByStatusAsync(OrderStatus status);
    }

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order.API.Models.Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Bill)
                .ToListAsync();
        }

        public async Task<Order.API.Models.Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Bill)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order.API.Models.Order> CreateOrderAsync(Order.API.Models.Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.OrderNumber = GenerateOrderNumber();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order.API.Models.Order> UpdateOrderAsync(Order.API.Models.Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Order.API.Models.Order>> GetPendingOrdersAsync()
        {
            return await GetOrdersByStatusAsync(OrderStatus.Pending);
        }

        public async Task<List<Order.API.Models.Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{{DateTime.Now:yyyyMMddHHmmss}}-{{new Random().Next(1000, 9999)}}";
        }
    }
}