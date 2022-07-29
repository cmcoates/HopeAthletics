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
    internal class ScheduleService
    {
        private string currentMonth;
        public String CurrentMonth
        {
            get { return currentMonth; }
            set { currentMonth = value; }
        }
        public ObservableCollection<Game> GetSchedule(int sport)
        {
            ObservableCollection<Game> schedule = new();
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
                        if (!match.Value.Equals(""))
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
                        opponent = string.Concat(opponent.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

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
                        schedule.Add(game);
                    }
                }

            }
            return schedule;
        }
    }
}
