using System.Diagnostics;
using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Repositories;
using BubbleTeaShop.Backend.Repositories.MenuItem;
using BubbleTeaShop.Backend.Services;
using BubbleTeaShop.Client.ViewModels;
using BubbleTeaShop.Client.Views;
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

        var envPath = Environment.GetEnvironmentVariable("BUBBLE_DB");
        string dbPath;

        if (!string.IsNullOrEmpty(envPath) && File.Exists(envPath))
        {
            dbPath = envPath;
            Debug.WriteLine($"[DEBUG] Using DB from BUBBLE_DB: {dbPath}");
        }
        else
        {
            dbPath = Path.Combine(FileSystem.AppDataDirectory, "BubbleTeaShop.db");

            if (!File.Exists(dbPath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("BubbleTeaShop.db").Result;
                using var dest = File.Create(dbPath);
                stream.CopyTo(dest);
                Debug.WriteLine($"[DEBUG] Copied DB to app container: {dbPath}");
            }
            else
            {
                Debug.WriteLine($"[DEBUG] Using existing app container DB: {dbPath}");
            }
        }
        
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
        
        builder.Services.AddTransient<OrderDetailsViewModel>();
        builder.Services.AddTransient<OrderDetailsPage>();

        builder.Services.AddTransient<SelectCategoryViewModel>();
        builder.Services.AddTransient<SelectCategoryPage>();
        
        builder.Services.AddTransient<SelectMenuItemViewModel>();
        builder.Services.AddTransient<SelectMenuItemPage>();

        builder.Services.AddTransient<CustomizeOrderLineViewModel>();
        builder.Services.AddTransient<CustomizeOrderLinePage>();

        builder.Services.AddTransient<OrderDetailsSummaryViewModel>();
        builder.Services.AddTransient<OrderDetailsSummaryPage>();
        
        builder.Services.AddTransient<AppShell>(sp => 
        {
            var mainPage = sp.GetRequiredService<MainPage>();
            return new AppShell(mainPage);
        });
        
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        return app;
    }
}