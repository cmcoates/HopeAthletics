using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        public String rosterNotFound = "";
        public String scheduleNotFound = "";

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

        public string RosterNotFound
        {
            get { return rosterNotFound; }
            set { rosterNotFound = value; OnPropertyChanged(); }
        }

        public string ScheduleNotFound
        {
            get { return scheduleNotFound; }
            set { scheduleNotFound = value; OnPropertyChanged(); }
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

        public static void GoHome()
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

            if(Games.Count == 0)
            {
                ScheduleNotFound = "Schedule not available";
            }
            else
            {
                ScheduleNotFound = "";
            }
            
            if (Athletes.Count == 0)
            {
                RosterNotFound = "Roster not available";
            }
            else
            {
                RosterNotFound = "";
            }
            RosterBtnVisibility = true;
            ScheduleBtnVisibility = true;
        }

        private void DisplaySports()
        {
            ObservableCollection<string> sports = new()
            {
                "Baseball",
                "Basketball",
                "Cross Country",
                "Football",
                "Golf",
                "Lacrosse",
                "Soccer",
                "Swimming & Diving",
                "Tennis",
                "Track & Field",
                "ACHA Hockey"
            };
            Sports = sports;
        }

        public AthleticsMenViewModel()
        {
            HomeCommand = new Command(() => AthleticsMenViewModel.GoHome());
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
