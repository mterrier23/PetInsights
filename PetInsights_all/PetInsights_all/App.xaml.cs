using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all
{
    public partial class App : Application
    {
        public App()
        {
            //InitializeComponent(); // was here originally

            //MainPage = new GetLocation(); // causes errors
            MainPage = new NavigationPage(new GetLocation()); // previously was working fine
            // possible going to have errors here due to update student stuff

            // NOTE - test making MainTabbed the mainpage, and putting GetLocation modally on top
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
