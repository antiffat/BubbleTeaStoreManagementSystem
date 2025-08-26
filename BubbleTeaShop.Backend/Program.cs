using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using BubbleTesShop.Backend.Helpers;

// Create a service collection to register services.
var services = new ServiceCollection();

// Add the DbContext to the service collection.
// We are using a simple hard-coded connection string for migrations.
services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=BubbleTeaShop.db");
    
    // Enable detailed SQL query logging for debugging.
    options.LogTo(Console.WriteLine, LogLevel.Information);
    options.EnableSensitiveDataLogging();
});

// Build the service provider. This is all the EF Core tools need to run migrations.
var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("EF Core services for migrations configured successfully.");