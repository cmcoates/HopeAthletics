using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Intramural
    {
        public string Sport { get; set; }
        public string Season { get; set; }
        public string Link { get; set; }
        public ObservableCollection<Standing> Standings { get; set; }
        public Schedule Schedule { get; set; }
    }
}
