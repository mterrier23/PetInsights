﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all
{
    public partial class App : Application
    {
        public App()
        {
            //InitializeComponent(); // was here originally

            // If user previously used app, we skip location page
            if (Application.Current.Properties.ContainsKey("UserLocation"))
            {
                MainPage = new NavigationPage(new MainTabbed()); // testing
            }
            else
            {
                MainPage = new NavigationPage(new GetLocation()); // previously was working fine
            }

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
