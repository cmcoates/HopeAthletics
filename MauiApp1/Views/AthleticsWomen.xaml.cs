namespace MauiApp1;

public partial class AthleticsWomen : ContentPage
{
	public AthleticsWomen()
	{
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new MainPage());
    }
}