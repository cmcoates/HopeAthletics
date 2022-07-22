using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MauiApp1.Models;

namespace MauiApp1;

public partial class AthleticsMen : ContentPage
{
    public ObservableCollection<String> Sports { get; set; } = new();

    public ObservableCollection<Athlete> Athletes { get; set; } = new();
    


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

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        Athletes.Clear();
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if(selectedIndex != -1)
        {
            GetRoster(selectedIndex);
        }
    }

    private void GetRoster(int sport)
    {
        HtmlWeb web = new();
        HtmlDocument docA = web.Load("https://athletics.hope.edu/navbar-men-sports");
        var mensSports = docA.DocumentNode.SelectNodes("//li[@class='has-nav']//ul[@class='clearfix']//li[2]");

        var msport = mensSports[sport];
        String path = msport.InnerHtml;
        string pattern = @"/sports/[\w/-]*";
        Regex rg = new(pattern);
        Match match = rg.Match(path);
        path = match.Value;

        HtmlDocument docB = web.Load("https://athletics.hope.edu" + path);
        var athls = docB.DocumentNode.SelectNodes("//div[@class='roster']//table//tbody//tr");
        foreach(var a in athls)
        {
            String htmlAthleteInfo = a.InnerHtml;
            htmlAthleteInfo = htmlAthleteInfo.Replace("\t", "");
            htmlAthleteInfo = htmlAthleteInfo.Replace("\n", "");
            htmlAthleteInfo = htmlAthleteInfo.Replace(" ", "");

            string athletePattern = "(?<=class=\"headshotlazyload\">)[\\w]*";
            Regex nameReg = new(athletePattern);
            match = nameReg.Match(htmlAthleteInfo);
            String name = match.Value;

            athletePattern = "(?<=Hometown/HighSchool:</span.)[\\w,/.]*(?<!</td>)";
            Regex homeReg = new(athletePattern);
            match = homeReg.Match(htmlAthleteInfo);
            String hometown = match.Value;

            athletePattern = "(?<=Class:</span>)[\\w]*";
            Regex classReg = new(athletePattern);
            match = classReg.Match(htmlAthleteInfo);
            String athleteClass = match.Value;

            Athlete athlete = new()
            {
                Name = name,
                Hometown = hometown,
                Class = athleteClass
            };
            Athletes.Add(athlete);
        }
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
    }
}