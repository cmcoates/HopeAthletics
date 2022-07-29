using HtmlAgilityPack;
using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    internal class RosterService
    {
        public ObservableCollection<Athlete> GetRoster(int selectedIndex)
        {
            ObservableCollection<Athlete> athletes = new();
            HtmlWeb web = new();
            HtmlDocument docA = web.Load("https://athletics.hope.edu/navbar-men-sports");
            var mensSports = docA.DocumentNode.SelectNodes("//li[@class='has-nav']//ul[@class='clearfix']//li[2]");
            mensSports.RemoveAt(10);
            var msport = mensSports[selectedIndex];
            String path = msport.InnerHtml;
            string pattern = @"/sports/[\w/-]*";
            Regex rg = new(pattern);
            Match match = rg.Match(path);
            path = match.Value;

            HtmlDocument docB = web.Load("https://athletics.hope.edu" + path);
            var athls = docB.DocumentNode.SelectNodes("//div[@class='roster']//table//tbody//tr");
            if (athls is not null)
            {
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
                    athletes.Add(athlete);
                }
            }
            return athletes;
        }
    }
}
