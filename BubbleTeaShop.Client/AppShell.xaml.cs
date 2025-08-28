using BubbleTeaShop.Client.ViewModels;
using Microsoft.Maui.Controls;

namespace BubbleTeaShop.Client;

public partial class AppShell : Shell
{
    public AppShell(MainPage mainPage)
    {
        InitializeComponent();
        Items.Add(new ShellContent
        {
            Content = mainPage
        });
    }
}