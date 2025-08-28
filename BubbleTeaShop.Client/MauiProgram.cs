using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Client.ViewModels;
using BubbleTesShop.Backend.Repositories;
using BubbleTesShop.Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BubbleTeaShop.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var dbPath = Path.Combine(
            FileSystem.AppDataDirectory, 
            "BubbleTeaShop.db");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite($"Data Source={dbPath}");
#if DEBUG
            options.LogTo(System.Console.WriteLine, LogLevel.Information);
            options.EnableSensitiveDataLogging();
#endif
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IOrderLineRepository, OrderLineRepository>();
        builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        builder.Services.AddScoped<IFrappeMenuItemRepository, FrappeMenuItemRepository>();
        builder.Services.AddScoped<IMilkTeaMenuItemRepository, MilkTeaMenuItemRepository>();
        builder.Services.AddScoped<IFruitTeaMenuItemRepository, FruitTeaMenuItemRepository>();
        builder.Services.AddScoped<IStoreRepository, StoreRepository>();
        builder.Services.AddScoped<IAssignmentHistoryRepository, AssignmentHistoryRepository>();
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
        
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderLineService, OrderLineService>();
        builder.Services.AddScoped<IMenuItemService, MenuItemService>();
        builder.Services.AddScoped<IStoreService, StoreService>();
        builder.Services.AddScoped<IAssignmentHistoryService, AssignmentHistoryService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IShiftService, ShiftService>();

        builder.Services.AddTransient<OrderHistoryViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<AppShell>();
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        return app;
    }
}