using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTesShop.Backend.Repositories;

public class OrderLineRepository : IOrderLineRepository
{
    private readonly ApplicationDbContext _context;
    
    public OrderLineRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderLine>> GetAllOrderLinesAsync()
    {
        return await _context.OrderLines
            .Include(ol => ol.MenuItem)
            .Include(ol => ol.Order)
            .Include(ol => ol.OrderLineToppings)
            .ToListAsync();
    }
    
    public async Task<OrderLine> GetOrderLineByIdAsync(int id)
    {
        return await _context.OrderLines
            .Include(ol => ol.MenuItem)
            .Include(ol => ol.Order)
            .Include(ol => ol.OrderLineToppings)
            .FirstOrDefaultAsync(ol => ol.Id == id);
    }
    
    public async Task AddOrderLineAsync(OrderLine orderLine)
    {
        await _context.OrderLines.AddAsync(orderLine);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateOrderLineAsync(OrderLine orderLine)
    {
        _context.OrderLines.Update(orderLine);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteOrderLineAsync(int id)
    {
        var orderLine = await _context.OrderLines.FirstOrDefaultAsync(ol => ol.Id == id);
        if (orderLine != null)
        {
            _context.OrderLines.Remove(orderLine);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> OrderLineExistsAsync(int id)
    {
        return await _context.OrderLines.AnyAsync(ol => ol.Id == id);
    }
    
    public async Task<IEnumerable<OrderLine>> GetOrderLineByMenuItemIdAsync(int menuItemId)
    {
        return await _context.OrderLines
            .Where(ol => ol.MenuItemId == menuItemId)
            .Include(ol => ol.MenuItem)
            .Include(ol => ol.Order)
            .Include(ol => ol.OrderLineToppings)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<OrderLine>> GetOrderLineByOrderIdAsync(int orderId)
    {
        return await _context.OrderLines
            .Where(ol => ol.OrderId == orderId)
            .Include(ol => ol.MenuItem)
            .Include(ol => ol.Order)
            .Include(ol => ol.OrderLineToppings)
            .ToListAsync();
    }
    
    public async Task<bool> OrderLineExistsMenuItemOrderAsync(int menuItemId, int orderId)
    {
        return await _context.OrderLines.AnyAsync(ol => ol.MenuItemId == menuItemId && ol.OrderId == orderId);
    }
}