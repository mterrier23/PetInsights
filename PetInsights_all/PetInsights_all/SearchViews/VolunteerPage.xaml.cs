using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all.SearchViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VolunteerPage : ContentPage
    {
        public VolunteerPage()
        {
            InitializeComponent();
            // Shade in the Volunteer Button
            volunteerButton.BackgroundColor = Color.Blue;
            volunteerButton.BorderColor = Color.Blue;
            volunteerButton.TextColor = Color.White;

            // Shade out the Donate Button
            donateButton.BackgroundColor = Color.White;
            donateButton.BorderColor = Color.LightGray;
            donateButton.TextColor = Color.Black;
            
        }
    }
}