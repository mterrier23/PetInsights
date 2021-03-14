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

namespace PetInsights_all.OrgSide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrgPetView : ContentPage
    {
        public OrgPetView()
        {
            InitializeComponent();
            BindingContext = new PetsViewModel();
        }

        async void lstPets_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            ObservableCollection<Pet> petList = lstPets.ItemsSource as ObservableCollection<Pet>;

            var pet = e.CurrentSelection.First() as Pet;

            if (pet == null)
            {
                Console.WriteLine("Pet is Null");
                return;
            }

            await Navigation.PushAsync(new PetDetailsPage(pet, petList));
        }
        async void btnAddPet_clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddPet());
        }
    }
}