using PetInsights_all.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PetInsights_all.ViewModel;
using PetInsights_all.Search;
using System.Collections.ObjectModel;

namespace PetInsights_all
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class templist : ContentPage
    {

        public templist()
        {
            InitializeComponent();

            BindingContext = new PetsViewModel();
        }


        async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public async void OnItemSelected(object sender, ItemTappedEventArgs args)
        {
            var pet = args.Item as Pet;
            if (pet == null) return;

            await Navigation.PushAsync(new PetDetailsPage(pet));
            lstPets.SelectedItem = null;
        }
    }
}