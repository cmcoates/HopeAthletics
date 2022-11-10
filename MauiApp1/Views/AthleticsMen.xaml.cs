using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using MauiApp1.Models;

namespace MauiApp1;

public partial class AthleticsMen : ContentPage
{
    private readonly AthleticsMenViewModel vm;
    
    public AthleticsMen()
    {
        InitializeComponent();
        vm = BindingContext as AthleticsMenViewModel ?? new AthleticsMenViewModel();
    }

    private void SportPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            vm.GetTeam(selectedIndex);
        }
    }
}