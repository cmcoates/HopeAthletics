using HtmlAgilityPack;
using MauiApp1.Models;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1
{
    public class AthleticsMenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand RosterCommand { get; private set; }
        public ICommand ScheduleCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }

        readonly RosterService rosterService = new();
        readonly ScheduleService scheduleService = new();

        private ObservableCollection<Athlete> athletes;
        private ObservableCollection<string> sports;
        private ObservableCollection<Game> games;
        
        public int pickerItem;
        public bool rosterVisibility = false;
        public bool scheduleVisibility = false;
        public bool rosterBtnVisibility = false;
        public bool scheduleBtnVisibility = false;

        public ObservableCollection<Athlete> Athletes
        {
            get { return athletes; }
            set { athletes = value; OnPropertyChanged(); }
        }

        public ObservableCollection<String> Sports
        {
            get { return sports; }
            set { sports = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Game> Games
        {
            get { return games; }
            set { games = value; OnPropertyChanged(); }
        }

        public int PickerItem
        {
            get { return pickerItem; }
            set { pickerItem = value; GetTeam(pickerItem); OnPropertyChanged(); }
        }

        public bool RosterVisibility
        {
            get { return rosterVisibility; }
            set { rosterVisibility = value; OnPropertyChanged(); }
        }

        public bool ScheduleVisibility
        {
            get { return scheduleVisibility; }
            set { scheduleVisibility = value; OnPropertyChanged(); }
        }

        public bool RosterBtnVisibility
        {
            get { return rosterBtnVisibility; }
            set { rosterBtnVisibility = value; OnPropertyChanged(); }
        }

        public bool ScheduleBtnVisibility
        {
            get { return scheduleBtnVisibility; }
            set { scheduleBtnVisibility = value; OnPropertyChanged(); }
        }

        public void GetRoster()
        {
            RosterBtnVisibility = true;
            ScheduleBtnVisibility = true;
            RosterVisibility = true;
            ScheduleVisibility = false;
        }

        public void GetSchedule()
        {
            RosterBtnVisibility = true;
            ScheduleBtnVisibility = true;
            RosterVisibility = false;
            ScheduleVisibility = true;
        }

        public void GoHome()
        {
            if (App.Current is null)
                return;
            App.Current.MainPage = new NavigationPage(new MainPage());
        }

        public void GetTeam(int sport)
        {
            RosterBtnVisibility = false;
            ScheduleBtnVisibility = false;
            Games = scheduleService.GetSchedule(sport);
            Athletes = rosterService.GetRoster(sport);
            RosterBtnVisibility = true;
            ScheduleBtnVisibility = true;
        }

        private void DisplaySports()
        {
            ObservableCollection<string> sports = new();
            sports.Add("Baseball");
            sports.Add("Basketball");
            sports.Add("Cross Country");
            sports.Add("Football");
            sports.Add("Golf");
            sports.Add("Lacrosse");
            sports.Add("Soccer");
            sports.Add("Swimming & Diving");
            sports.Add("Tennis");
            sports.Add("Track & Field");
            sports.Add("ACHA Hockey");
            Sports = sports;
        }

        public AthleticsMenViewModel()
        {
            HomeCommand = new Command(() => GoHome());
            RosterCommand = new Command(() => GetRoster());
            ScheduleCommand = new Command(() => GetSchedule());
            DisplaySports();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
