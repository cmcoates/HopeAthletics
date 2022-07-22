namespace MauiApp1;

public partial class About : ContentPage
{
	public About()
	{
		InitializeComponent();
        this.BindingContext = this;
    }

	private void Button_Clicked(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new MainPage());
    }
}