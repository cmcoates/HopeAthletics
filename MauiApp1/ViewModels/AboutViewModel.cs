using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1
{
    public class AboutViewModel
    {
        public ICommand HomeCommand { get; private set; }
        public ICommand DonateCommand { get; private set; }

        private void GoHome()
        {
            if (App.Current is null)
                return;
            App.Current.MainPage = new NavigationPage(new MainPage());
        }

        private async void Donate()
        {
            try
            {
                Uri uri = new Uri("https://www.microsoft.com");
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        public AboutViewModel()
        {
            HomeCommand = new Command(() => GoHome());
            DonateCommand = new Command(() => Donate());
        }

    }
}
