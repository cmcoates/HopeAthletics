namespace MauiApp1;

public partial class AthleticsMen : ContentPage
{
	public AthleticsMen()
	{
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new MainPage());
    }
}