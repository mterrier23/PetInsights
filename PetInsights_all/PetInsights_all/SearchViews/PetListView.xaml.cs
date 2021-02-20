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
    public partial class PetListView : ContentPage
    {

        public PetListView()
        {
            InitializeComponent();
            BindingContext = new PetsViewModel();
            Console.WriteLine("In pet list view");
        }

     
        async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public async void OnItemSelected(object sender, ItemTappedEventArgs args)
        {
            Console.WriteLine("on Item Selected");
            var pet = args.Item as Pet;
            if (pet == null) return;

            await Navigation.PushAsync(new PetDetailsPage(pet));
            lstPets.SelectedItem = null;
        }

    }
}