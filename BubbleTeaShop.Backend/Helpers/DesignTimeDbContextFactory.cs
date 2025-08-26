using BubbleTeaShop.Backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BubbleTesShop.Backend.Helpers;

public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Create DbContextOptions using a hard-coded connection string.
        // This is just for design-time use (migrations).
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlite("Data Source=BubbleTeaShop.db");

        // Return a new instance of the DbContext with the configured options.
        return new ApplicationDbContext(builder.Options);
    }
}