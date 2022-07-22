using HtmlAgilityPack;
using System.Collections.ObjectModel;

namespace MauiApp1;

public partial class AthleticsMen : ContentPage
{
    ObservableCollection<String> sports = new();
    

    public ObservableCollection<String> Sports
    {
        get { return sports; }
        set { sports = value; }
    }
    public String Text { get; set; }

    public AthleticsMen()
	{
		InitializeComponent();
        this.BindingContext = this;
        DisplaySports();
    }

	private void Button_Clicked(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new MainPage());
    }

    private void DisplaySports()
	{
        Sports.Clear();
        Sports.Add("Baseball");
        Sports.Add("Basketball");
        Sports.Add("Cross Country");
        Sports.Add("Football");
        Sports.Add("Golf");
        Sports.Add("Lacrosse");
        Sports.Add("Soccer");
        Sports.Add("Swimming & Diving");
        Sports.Add("Tennis");
        Sports.Add("Track & Field");
        Sports.Add("ACHA Hockey");
        Text = "Show shomething";
    }
}