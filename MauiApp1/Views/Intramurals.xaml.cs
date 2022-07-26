using HtmlAgilityPack;
using MauiApp1.Models;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace MauiApp1;

public partial class Intramurals : ContentPage
{
    public ObservableCollection<Intramural> Events { get; set; } = new();
    public ObservableCollection<Standing> Stands { get; set; } = new();
    public string CurrentDate { get; set; }


    public Intramurals()
	{
		InitializeComponent();
        this.BindingContext = this;
        GetSports();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MainPage());
    }

   

    public void GetSports()
    {
        Events.Clear();
        HtmlWeb web = new();
        HtmlDocument docA = web.Load("https://scheduler.leaguelobster.com/o/310363/hope-college-intramurals/");
        var intramurals = docA.DocumentNode.SelectNodes("//div[@class='season-card border rounded px-4 py-5 my-4 hover-box-shadow']");

        foreach (var sport in intramurals)
        {
            String sportInfo = sport.InnerHtml;
            sportInfo = sportInfo.Replace("\t", "");
            sportInfo = sportInfo.Replace("\n", "");
            //sportInfo = sportInfo.Replace(" ", "");

            string pattern = "(?<=league-title\">)[\\w\\s/]*";
            Regex nameReg = new(pattern);
            Match match = nameReg.Match(sportInfo);
            String name = match.Value;

            pattern = "(?<=season-title mb-5\">)[\\w\\s/]*";
            Regex seasonReg = new(pattern);
            match = seasonReg.Match(sportInfo);
            string season = match.Value;

            pattern = "(?<=<a class=\"btn btn-outline-primary\" href=\")[\\w/-]*";
            Regex urlReg = new(pattern);
            match = urlReg.Match(sportInfo);
            string url = match.Value;

            

            docA = web.Load("https://scheduler.leaguelobster.com/" + url);
            var intramuralInfo = docA.DocumentNode.SelectNodes("//div[@class='division-standings']//div[@class='row']//table[@class='table table-hover table-condensed no-borders']//tbody");
            
            string standingInfo = intramuralInfo.First().InnerHtml;
            standingInfo = standingInfo.Replace("\t", "");
            standingInfo = standingInfo.Replace("\n", "");
            standingInfo = standingInfo.Replace(" ", "");

            string[] standingsTable = standingInfo.Split("<tdclass=\"text-left\">");

            ObservableCollection<Standing> standings = new();
            for(int i = 1; i < standingsTable.Length; i++)
            {
                string str = standingsTable[i];

                pattern = "[\\d]*";
                Regex reg = new(pattern);
                match = reg.Match(str);
                string ranking = match.Value;

                pattern = "(?<=/team/[\\d]*/)[\\w-]*";
                reg = new(pattern);
                match = reg.Match(str);
                string team = match.Value;

                pattern = "(?<=games_played\">)[\\d]*";
                reg = new(pattern);
                match = reg.Match(str);
                string gamesPlayed = match.Value;

                pattern = "(?<=games_played\">[\\d]*</td><td>)[\\d]*";
                reg = new(pattern);
                match = reg.Match(str);
                string wins = match.Value;
                if (wins.Equals(""))
                {
                    wins = "0";
                }

                pattern = "(?<=games_played\">[\\d]*</td><td>[\\d]*</td><td>[\\d]*)";
                reg = new(pattern);
                match = reg.Match(str);
                string losses = match.Value;
                if (losses.Equals(""))
                {
                    losses = "0";
                }

                pattern = "(?<=tdclass=\"ties\">)[\\d]*";
                reg = new(pattern);
                match = reg.Match(str);
                string ties = match.Value;
                if (ties.Equals(""))
                {
                    ties = "0";
                }


                //docA = web.Load("https://scheduler.leaguelobster.com/" + url);
                //intramuralInfo = docA.DocumentNode.SelectNodes("//div[@class='col-md-7']//div[@class='panel panel-default schedule-panel schedule-maker-panel']//div[@class='panel-body schedule games-container']//div//div//div");
                //var tag = docA.GetElementbyId("week").ChildNodes;

                Standing standing = new()
                {
                    Ranking = ranking,
                    Team = team,
                    GamesPlayed = gamesPlayed,
                    Wins = wins,
                    Losses = losses,
                    Draws = ties
                };
                standings.Add(standing);
            }
            Intramural intramural = new()
            {
                Sport = name,
                Season = season,
                Link = url,
                Standings = standings
            };

            Events.Add(intramural);
        }
    }

    private void IntramuralEvents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        Stands.Clear();
        SportsBtn.IsVisible = true;
        Intramural item = e.SelectedItem as Intramural;
        ObservableCollection<Standing> standings = new();
        standings = item.Standings;
        foreach(Standing standing in standings)
        {
            Stands.Add(standing);
        }
        IntramuralEvents.IsVisible = false;
        IntramuralStandings.IsVisible = true;
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        IntramuralEvents.IsVisible = true;
        IntramuralStandings.IsVisible = false;
        SportsBtn.IsVisible = false;
    }
}