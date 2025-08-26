using Microsoft.Maui.Controls;

namespace BubbleTeaShop.Client;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Example: navigate to a simple ContentPage
        Items.Add(new ShellContent
        {
            Content = new MainPage()
        });
    }
}