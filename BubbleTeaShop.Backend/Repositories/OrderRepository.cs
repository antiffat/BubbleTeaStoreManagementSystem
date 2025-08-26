using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTesShop.Backend.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    
    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.MenuItem)
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.OrderLineToppings)
            .ToListAsync();
    }
    
    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.MenuItem)
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.OrderLineToppings)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    
    public async Task AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> OrderExistsAsync(int id)
    {
        return await _context.Orders.AnyAsync(o => o.Id == id);
    }
}