using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PetInsights_all.OrgSide;

namespace PetInsights_all
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSearch : ContentPage
    {
        public MainSearch()
        {
            InitializeComponent();
        }
        async void PetMapButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PetMapView()); // NOTE - change this bc don't need navigation for this page
        }

        async void PetListButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PetListView()); // NOTE - change this bc don't need navigation for this page
        }

        async void AddPetButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPet()); // NOTE - change this bc don't need navigation for this page
        }


    }
}