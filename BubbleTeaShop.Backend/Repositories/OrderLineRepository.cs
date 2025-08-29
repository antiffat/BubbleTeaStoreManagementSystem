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
            .AsSplitQuery() 
            .Include(ol => ol.MenuItem)
            .Include(ol => ol.Order)
            .Include(ol => ol.OrderLineToppings)
            .ToListAsync();
    }
    
    public async Task<OrderLine> GetOrderLineByIdAsync(int id)
    {
        return await _context.OrderLines
            .AsSplitQuery() 
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
            .AsSplitQuery() 
            .Where(ol => ol.MenuItemId == menuItemId)
            .Include(ol => ol.MenuItem)
            .Include(ol => ol.Order)
            .Include(ol => ol.OrderLineToppings)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<OrderLine>> GetOrderLineByOrderIdAsync(int orderId)
    {
        return await _context.OrderLines
            .AsSplitQuery() 
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
    
    // 
    public async Task<OrderLine> GetOrderLineWithOrderAndOrderLinesAsync(int id)
    {
        return await _context.OrderLines
            .AsSplitQuery() 
            .Where(ol => ol.Id == id)
            .Include(ol => ol.MenuItem)
            .Include(ol => ol.Order)
            .ThenInclude(o => o.OrderLines) // <- important: includes order.OrderLines so we can check count in memory
            .Include(ol => ol.OrderLineToppings)
            .FirstOrDefaultAsync(ol => ol.Id == id);
    }
    
    public async Task<bool> TryDeleteOrderLineIfOrderHasMoreThanOneAsync(int orderLineId)
    {
        using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var ol = await _context.OrderLines
            .Where(x => x.Id == orderLineId)
            .Include(x => x.Order)
            .ThenInclude(o => o.OrderLines)
            .FirstOrDefaultAsync();

        if (ol == null) return false;

        if (ol.Order == null)
            throw new InvalidOperationException("Data integrity error: parent order missing");

        if (ol.Order.OrderLines?.Count <= 1)
            throw new InvalidOperationException("Cannot delete last orderline of an order.");

        _context.OrderLines.Remove(ol);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return true;
    }
}