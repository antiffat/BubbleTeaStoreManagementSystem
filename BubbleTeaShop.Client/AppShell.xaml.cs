using BubbleTeaShop.Client.ViewModels;
using BubbleTeaShop.Client.Views;
using Microsoft.Maui.Controls;

namespace BubbleTeaShop.Client;

public partial class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        Routing.RegisterRoute("OrderDetailsPage", typeof(OrderDetailsPage));
        Routing.RegisterRoute("SelectCategoryPage", typeof(SelectCategoryPage));
        Routing.RegisterRoute("SelectMenuItemPage", typeof(SelectMenuItemPage));
        Routing.RegisterRoute("CustomizeOrderLinePage", typeof(CustomizeOrderLinePage));
        Items.Add(new ShellContent
        {
            Content = mainPage
        });
    }
}