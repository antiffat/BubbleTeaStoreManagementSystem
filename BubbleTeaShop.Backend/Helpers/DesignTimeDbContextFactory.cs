using BubbleTeaShop.Backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BubbleTesShop.Backend.Helpers;

public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlite("Data Source=BubbleTeaShop.db");

        return new ApplicationDbContext(builder.Options);
    }
}