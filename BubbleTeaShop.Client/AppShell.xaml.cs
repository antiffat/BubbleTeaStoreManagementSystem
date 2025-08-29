using BubbleTeaShop.Client.ViewModels;
using BubbleTeaShop.Client.Views;
using Microsoft.Maui.Controls;

namespace BubbleTeaShop.Client;

public partial class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        // Routing.RegisterRoute("MainPage", typeof(MainPage));
        // Routing.RegisterRoute("OrderDetailsPage", typeof(OrderDetailsPage));
        // Routing.RegisterRoute("SelectCategoryPage", typeof(SelectCategoryPage));
        // Routing.RegisterRoute("SelectMenuItemPage", typeof(SelectMenuItemPage));
        // Routing.RegisterRoute("CustomizeOrderLinePage", typeof(CustomizeOrderLinePage));
        // Routing.RegisterRoute("OrderDetailsSummaryPage", typeof(OrderDetailsSummaryPage));
        Routing.RegisterRoute(nameof(SelectCategoryPage), typeof(SelectCategoryPage));
        Routing.RegisterRoute(nameof(SelectMenuItemPage), typeof(SelectMenuItemPage));
        Routing.RegisterRoute(nameof(CustomizeOrderLinePage), typeof(CustomizeOrderLinePage));
        Routing.RegisterRoute(nameof(OrderDetailsSummaryPage), typeof(OrderDetailsSummaryPage));
        Items.Add(new ShellContent
        {
            Content = mainPage,
            Route = "MainPage" // Ensure the route matches
        });
    }
}