using HtmlAgilityPack;
using MauiApp1.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MauiApp1;

public partial class AthleticsWomen : ContentPage
{
    public ObservableCollection<String> Sports { get; set; } = new();

    public ObservableCollection<Athlete> Athletes { get; set; } = new();

    public AthleticsWomen()
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

        if (selectedIndex != -1)
        {
            GetRoster(selectedIndex);
        }
    }

    private void GetRoster(int sport)
    {
        HtmlWeb web = new();
        HtmlDocument docA = web.Load("https://athletics.hope.edu/navbar-women-sport");
        var womensSports = docA.DocumentNode.SelectNodes("//li[@class='has-nav']//ul[@class='clearfix']//li[2]");
        //mensSports.RemoveAt(10);
        var msport = womensSports[sport];
        String path = msport.InnerHtml;
        string pattern = @"/sports/[\w/-]*";
        Regex rg = new(pattern);
        Match match = rg.Match(path);
        path = match.Value;

        HtmlDocument docB = web.Load("https://athletics.hope.edu" + path);
        var athls = docB.DocumentNode.SelectNodes("//div[@class='roster']//table//tbody//tr");
        if (athls is not null)
        {
            NotFound.Text = "";
            foreach (var a in athls)
            {
                String htmlAthleteInfo = a.InnerHtml;
                htmlAthleteInfo = htmlAthleteInfo.Replace("\t", "");
                htmlAthleteInfo = htmlAthleteInfo.Replace("\n", "");
                htmlAthleteInfo = htmlAthleteInfo.Replace(" ", "");

                string athletePattern = "(?<=class=\"headshotlazyload\">)[\\w.]*";
                Regex nameReg = new(athletePattern);
                match = nameReg.Match(htmlAthleteInfo);
                String name = match.Value;
                name = string.Concat(name.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

                athletePattern = "(?<=Hometown/HighSchool:</span.)[\\w,/.]*(?<!</td>)";
                Regex homeReg = new(athletePattern);
                match = homeReg.Match(htmlAthleteInfo);
                String hometown = match.Value;
                hometown = Regex.Replace(hometown, "([a-z])([A-Z])", "$1 $2");
                hometown = hometown.Replace(",", ", ");

                athletePattern = "(?<=Class:</span>)[\\w]*";
                Regex classReg = new(athletePattern);
                match = classReg.Match(htmlAthleteInfo);
                String athleteClass = match.Value;

                athletePattern = "(?<=data-src=\")[\\w/.-]*";
                Regex imgReg = new(athletePattern);
                match = imgReg.Match(htmlAthleteInfo);
                String img = match.Value;
                img = "https://athletics.hope.edu" + img;

                Athlete athlete = new()
                {
                    Name = name,
                    Hometown = hometown,
                    Class = athleteClass,
                    ImageUrl = img
                };
                Athletes.Add(athlete);
            }
        }
        else
        {
            NotFound.Text = "Roster not available";
        }
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