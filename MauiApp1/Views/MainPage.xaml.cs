
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static MauiApp1.MainPage;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
        this.BindingContext = this;
    }

	private void OnCounterClickedMenSports(object sender, EventArgs e)
	{
        App.Current.MainPage = new NavigationPage(new AthleticsMen());
    }
    private void OnCounterClickedWomensSports(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AthleticsWomen());
    }

    private void OnAboutClicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new About());
    }
}
