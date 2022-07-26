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
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1
{
    public class AthleticsMenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand PickerSelect { get; private set; }
        public ICommand RosterCommand { get; private set; }
        public ICommand ScheduleCommand { get; private set; }


        RosterService rosterService = new RosterService();
        ScheduleService scheduleService = new ScheduleService();

        private ObservableCollection<Athlete> athletes;
        private ObservableCollection<string> sports;
        private ObservableCollection<Game> games;
        private string currentMonth;

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
        public String CurrentMonth
        {
            get { return currentMonth; }
            set { currentMonth = value; OnPropertyChanged(); }
        }

        public void GetRoster()
        {
            Athletes = rosterService.GetRoster();
            Athletes.Add(new Athlete());
        }

        public ObservableCollection<Game> GetSchedule()
        {
            Game game = new Game();
            game.Opponent = "K";
            Games.Add(game);

            return Games;
        }


        public AthleticsMenViewModel()
        {
            RosterCommand = new Command(() => GetRoster());
            ScheduleCommand = new Command(() => GetSchedule());
        }











        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
