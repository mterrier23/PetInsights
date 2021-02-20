using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using PetInsights_all.OrgSide;
using PetInsights_all.SearchViews;

namespace PetInsights_all
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSearch : ContentPage
    {
        public MainSearch()
        {
            InitializeComponent();
            location.Text = Application.Current.Properties["UserLocation"].ToString();
        }

        // code already exists in GetLocation.xaml.cs
        private bool ConfirmLocation(string location)
        {
            // NOTE - found this regex online
            if (Regex.IsMatch(location, "^[0-9]{5}(?:-[0-9]{4})?$"))
                return true;
            else
                return false;
        }


        public void OnEnterPressed(object sender, EventArgs e)
        {
            bool realZip = false;
            realZip = ConfirmLocation(location.Text);
            while (realZip == false)
            {
                // Add a pop up asking "Invalid Zip Code, please re-enter a valid zipcode"
                DisplayAlert("Alert", "Invalid Zipcode entered. Please re-enter a valid zip code", "OK");
                /*Device.BeginInvokeOnMainThread(() => {
                    DisplayAlert(e.Title, e.Message, "OK");
                });*/
                realZip = ConfirmLocation(location.Text);
                location.Text = string.Empty;
            }

            DisplayAlert("Good", "Valid zip code typed", "OK");
            Application.Current.Properties["UserLocation"] = location.Text;
            Application.Current.SavePropertiesAsync();
        }

        async void PetMapButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PetMapView()); 
        }

        async void PetListButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PetListView()); 
        }

        async void AddPetButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPet()); 
        }


    }
}