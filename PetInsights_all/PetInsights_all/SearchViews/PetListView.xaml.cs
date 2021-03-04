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


        public async void OnTapGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Tap Success!");
            await Navigation.PopAsync();
            //await Navigation.PushAsync(new PetDetailsPage(pet))
        }

        async void lstPets_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            Console.WriteLine("on Item Selected");

            var pet = e.CurrentSelection.First() as Pet;
            Console.WriteLine(pet.Name);

            if (pet == null)
            {
                Console.WriteLine("Pet is Null");
                return;
            }

            await Navigation.PushAsync(new PetDetailsPage(pet));
         
        }
        

    }
}