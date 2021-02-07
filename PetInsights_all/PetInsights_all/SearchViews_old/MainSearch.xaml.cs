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
    public partial class MainSearch : ContentPage
    {
        public MainSearch()
        {
            Console.WriteLine("in mainsearch");
            //InitializeComponent();
        }
        async void PetMapButton_OnClicked(object sender, EventArgs e)
        {
            Console.WriteLine("pet map button clicked");
            await Navigation.PushAsync(new PetMapView()); // NOTE - change this bc don't need navigation for this page
        }
    }
}