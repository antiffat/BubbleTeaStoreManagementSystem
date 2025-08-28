namespace BubbleTeaShop.Client;

public partial class App : Application
{
	public App(AppShell appShell) 
	{
		InitializeComponent();

		MainPage = appShell;
	}
}
