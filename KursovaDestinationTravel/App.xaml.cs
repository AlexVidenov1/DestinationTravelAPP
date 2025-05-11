using System.Diagnostics;
using DestinationTravelAPP.Pages;
using DestinationTravelAPP.Services;

namespace DestinationTravelAPP
{
    public partial class App : Application
    {
        public App(DatabaseService db)
        {
            Debug.WriteLine(">>> Test here");
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage(db));
        }
    }
}
