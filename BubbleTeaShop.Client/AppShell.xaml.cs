using BubbleTeaShop.Client.ViewModels;
using BubbleTeaShop.Client.Views;
using Microsoft.Maui.Controls;

namespace BubbleTeaShop.Client;

public partial class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));        
        Items.Add(new ShellContent
        {
            Content = mainPage
        });
    }
}