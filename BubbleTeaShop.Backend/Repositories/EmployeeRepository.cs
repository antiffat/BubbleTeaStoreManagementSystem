using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTeaShop.Backend.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all employees from the database, including their associated assignment histories.
    /// </summary>
    /// <returns>A collection of all Employee entities.</returns>
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees
            .Include(e => e.AssignmentHistories)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves an employee by their unique identifier, including their associated assignment histories.
    /// </summary>
    /// <param name="id">The unique identifier of the employee.</param>
    /// <returns>The Employee entity or null if not found.</returns>
    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.AssignmentHistories)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <summary>
    /// Adds a new employee to the database.
    /// </summary>
    /// <param name="employee">The Employee entity to add.</param>
    public async Task AddEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing employee in the database.
    /// </summary>
    /// <param name="employee">The Employee entity to update.</param>
    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an employee from the database by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the employee to delete.</param>
    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Checks if an employee with the specified unique identifier exists in the database.
    /// </summary>
    /// <param name="id">The unique identifier of the employee.</param>
    /// <returns><c>true</c> if the employee exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> EmployeeExistsAsync(int id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }
}