using System.Collections.ObjectModel;

namespace MauiApp1;

public partial class AthleticsWomen : ContentPage
{
    ObservableCollection<String> sports = new();

    public ObservableCollection<String> Sports
    {
        get { return sports; }
        set { sports = value; }
    }

    public AthleticsWomen()
	{
		InitializeComponent();
		
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new MainPage());
    }

	private void DisplaySports()
	{
        Sports.Clear();
        
        Sports.Add("Basketball");
        Sports.Add("Cross Country");
        Sports.Add("Golf");
        Sports.Add("Lacrosse");
        Sports.Add("Soccer");
        Sports.Add("Softball");
        Sports.Add("Swimming & Diving");
        Sports.Add("Tennis");
        Sports.Add("Track & Field");
        Sports.Add("Volleyball");
        Sports.Add("Cheerleading");
    }
}