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
    public partial class DonatePage : ContentPage
    {
        public DonatePage()
        {
            InitializeComponent();
            // Shade in the Donate Button
            donateButton.BackgroundColor = Color.Green;
            donateButton.BorderColor = Color.Green;
            donateButton.TextColor = Color.White;

            volunteerButton.BackgroundColor = Color.White;
            volunteerButton.BorderColor = Color.LightGray;
            volunteerButton.TextColor = Color.Black;
            
        }
    }
}