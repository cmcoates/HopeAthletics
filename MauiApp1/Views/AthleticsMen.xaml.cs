using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MauiApp1.Models;

namespace MauiApp1;

public partial class AthleticsMen : ContentPage
{
    public ObservableCollection<String> Sports { get; set; } = new();

    public ObservableCollection<Athlete> Athletes { get; set; } = new();

    public ObservableCollection<Game> Games { get; set; } = new();
    public String CurrentMonth { get; set; }

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
        Games.Clear();
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if(selectedIndex != -1)
        {
            GetRoster(selectedIndex);
            GetSchedule(selectedIndex);
        }
        RosterBtn.IsVisible = true;
        ScheduleBtn.IsVisible = true;
        RosterList.IsVisible = false;
        ScheduleList.IsVisible = false;
    }

    private void OnRosterClicked(object sender, EventArgs e)
    {
        ScheduleList.IsVisible = false;
        RosterList.IsVisible = true;
    }

    private void OnScheduleClicked(object sender, EventArgs e)
    {
        RosterList.IsVisible = false;
        ScheduleList.IsVisible = true;
    }

    private void GetRoster(int sport)
    {
        HtmlWeb web = new();
        HtmlDocument docA = web.Load("https://athletics.hope.edu/navbar-men-sports");
        var mensSports = docA.DocumentNode.SelectNodes("//li[@class='has-nav']//ul[@class='clearfix']//li[2]");
        mensSports.RemoveAt(10);
        var msport = mensSports[sport];
        String path = msport.InnerHtml;
        string pattern = @"/sports/[\w/-]*";
        Regex rg = new(pattern);
        Match match = rg.Match(path);
        path = match.Value;

        HtmlDocument docB = web.Load("https://athletics.hope.edu" + path);
        var athls = docB.DocumentNode.SelectNodes("//div[@class='roster']//table//tbody//tr");
        if (athls is not null)
        {
            RosterNotFound.Text = "";
            ScheduleNotFound.Text = "";
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
            RosterNotFound.Text = "Roster not available";
        }
    }

    private void GetSchedule(int sport)
    {
        HtmlWeb web = new();
        HtmlDocument docA = web.Load("https://athletics.hope.edu/navbar-men-sports");
        var mensSchedule = docA.DocumentNode.SelectNodes("//li[@class='has-nav']//ul[@class='clearfix']//li[1]");
        mensSchedule.RemoveAt(10);
        var msport = mensSchedule[sport];
        String path = msport.InnerHtml;
        string pattern = @"/sports/[\w/-]*";
        Regex rg = new(pattern);
        Match match = rg.Match(path);
        path = match.Value;

        HtmlDocument docB = web.Load("https://athletics.hope.edu" + path);
        var sched = docB.DocumentNode.SelectNodes("//div[@class='schedule-content clearfix']//table//tbody//tr");
        if (sched is not null)
        {
            ScheduleNotFound.Text = "";
            foreach (var a in sched)
            {
                String htmlScheduleInfo = a.InnerHtml;
                htmlScheduleInfo = htmlScheduleInfo.Replace("\t", "");
                htmlScheduleInfo = htmlScheduleInfo.Replace("\n", "");
                htmlScheduleInfo = htmlScheduleInfo.Replace(" ", "");

                if (htmlScheduleInfo.Contains("tdcolspan=\"6\""))
                {
                    string monthPattern = "(?<=<tdcolspan=\"6\">)[\\w]*";
                    Regex monthReg = new(monthPattern);
                    match = monthReg.Match(htmlScheduleInfo);
                    if(!match.Value.Equals(""))
                    {
                        CurrentMonth = match.Value;
                    }
                }
                else
                {
                    string schedulePattern = "(?<=<tdclass=\"e_date\">)[\\w.]*";
                    Regex dateReg = new(schedulePattern);
                    match = dateReg.Match(htmlScheduleInfo);
                    String date = match.Value;
                    date = CurrentMonth + " " + date;

                    schedulePattern = "(?<=<spanclass=\"va\">)[\\w]*";
                    Regex locationReg = new(schedulePattern);
                    match = locationReg.Match(htmlScheduleInfo);
                    String location = match.Value;

                    schedulePattern = "(?<=<spanclass=\"team-name\">)[\\w]*";
                    Regex opponentReg = new(schedulePattern);
                    match = opponentReg.Match(htmlScheduleInfo);
                    String opponent = match.Value;

                    schedulePattern = "(?<=<tdclass=\"e_result\">)[\\w,-]*";
                    Regex resultReg = new(schedulePattern);
                    match = resultReg.Match(htmlScheduleInfo);
                    String result = match.Value;
                    if (result.Equals(""))
                    {
                        result = "TBD";
                    }

                    Game game = new()
                    {
                        Date = date,
                        Location = location,
                        Opponent = opponent,
                        Result = result
                    };
                    Games.Add(game);
                }
            }
        }
        else
        {
            ScheduleNotFound.Text = "Schedule not available";
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