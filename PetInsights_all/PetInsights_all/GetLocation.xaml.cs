using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetInsights_all.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace PetInsights_all
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetLocation : ContentPage
    {
        public GetLocation()
        {
            InitializeComponent();
        }

        private async void FindPetsButton_OnClicked(object sender, EventArgs e)
        {
            bool realZip = false;
            realZip = ConfirmLocation(location.Text);
            while (realZip == false)
            {
                // Add a pop up asking "Invalid Zip Code, please re-enter a valid zipcode"
                realZip = ConfirmLocation(location.Text);
                location.Text = string.Empty;
            }
            // location.Text save as cached varible
            //updateDistanceonPets(location.Text);      // testing 
            location.Text = string.Empty;
            await Navigation.PushModalAsync(new MainTabbed());
        }

        private bool ConfirmLocation(string location)
        {
            // NOTE - found this regex online
            if (Regex.IsMatch(location, "^[0-9]{5}(?:-[0-9]{4})?$"))
                return true;
            else
                return false;
        }

        // To be used for determining nearby pets only -- more research needed!
        private void updateDistanceonPets(string location)
        {
            // GOAL: add a column to each pet and calculate distance using:
            Location userLoc = new Location(42.358056, -71.063611);
            Location sanFrancisco = new Location(37.783333, -122.416667);
            double miles = Location.CalculateDistance(userLoc, sanFrancisco, DistanceUnits.Miles);

        }

        private async void GoToOrgSite(object sender, EventArgs e)
        {
            // Logic for going to Org Site
        }
    }
}