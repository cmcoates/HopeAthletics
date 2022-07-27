namespace MauiApp1;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
        this.BindingContext = this;
    }

	private void OnCounterClickedMenSports(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new AthleticsMen());
    }
    private void OnCounterClickedWomensSports(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AthleticsWomen());
    }

    private void OnAboutClicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new About());
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new Intramurals());
    }
}
