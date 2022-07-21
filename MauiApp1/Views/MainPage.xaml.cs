
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static MauiApp1.MainPage;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    ObservableCollection<String> sports = new();

    public ObservableCollection<String> Sports
    {
        get { return sports; }
        set { sports = value;  }
    }

    public MainPage()
	{
		InitializeComponent();
        this.BindingContext = this;
    }

	private void OnCounterClickedMenSports(object sender, EventArgs e)
	{
        /*Sports.Clear();
        HtmlWeb web = new HtmlWeb();
        HtmlWeb athleticsWeb = new HtmlWeb();
        HtmlDocument docA = web.Load("https://athletics.hope.edu/navbar-men-sports");
        var mensSports = docA.DocumentNode.SelectNodes("//li[@class='has-nav']//a[1]");
        ObservableCollection<Sport> sports = new ObservableCollection<Sport>();
        
        foreach (var sport in mensSports)
        {
            if(!sport.InnerText.Equals("Schedule") && !sport.InnerText.Equals("Roster") && !sport.InnerText.Equals("Stats") && !sport.InnerText.Equals("Instagram") && !sport.InnerText.Equals("Facebook") && !sport.InnerText.Equals("Twitter"))
            {
                Sports.Add(new Sport { SportName = sport.InnerText });
            }
            
        }
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
        Sports.Add("ACHA Hockey");*/
        App.Current.MainPage = new NavigationPage(new AthleticsMen());
    }
    private void OnCounterClickedWomensSports(object sender, EventArgs e)
    {
        /*Sports.Clear();
        HtmlWeb web = new HtmlWeb();
        HtmlWeb athleticsWeb = new HtmlWeb();
        HtmlDocument docA = web.Load("https://athletics.hope.edu/navbar-women-sport");
        var mensSports = docA.DocumentNode.SelectNodes("//li[@class='has-nav']//a[1]");
        ObservableCollection<Sport> sports = new ObservableCollection<Sport>();

        foreach (var sport in mensSports)
        {
            if (!sport.InnerText.Equals("Schedule") && !sport.InnerText.Equals("Roster") && !sport.InnerText.Equals("Stats") && !sport.InnerText.Equals("Instagram") && !sport.InnerText.Equals("Facebook") && !sport.InnerText.Equals("Twitter") && !sport.InnerText.Equals("Times") || !sport.InnerText.Equals("Twitter"))
            {
                Sports.Add(new Sport { SportName = sport.InnerText });
            }

        }
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
        Sports.Add("Cheerleading");*/
        App.Current.MainPage = new NavigationPage(new AthleticsWomen());
    }

    private void OnAboutClicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new About());
    }
}
