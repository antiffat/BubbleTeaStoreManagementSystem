using System;
using BubbleTeaShop.Backend.Helpers;
using BubbleTesShop.Backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BubbleTesShop.Backend
{
    public class ProgramStandalone
    {
        public static void Main(string[] args)
        {
            // Create a service collection to register services.
            var services = new ServiceCollection();

            // Add the DbContext to the service collection.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("Data Source=BubbleTeaShop.db");
                options.LogTo(Console.WriteLine, LogLevel.Information);
                options.EnableSensitiveDataLogging();
            });

            // Build the service provider.
            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("EF Core services for migrations configured successfully.");
        }
    }
}