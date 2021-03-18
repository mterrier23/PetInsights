using PetInsights_all.Model;
using PetInsights_all.Search;
using PetInsights_all.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainFaves : ContentPage
    {

        ObservableCollection<Pet> FavePets = new ObservableCollection<Pet>();
        public MainFaves()
        {
            InitializeComponent();
            BindingContext = new PetsViewModel();
            Task.Delay(500); // Don't ask me why it works, it just does -- do not remove
            FindFaves();
        }

        public async void FindFaves()
        {
            await Task.Delay(1500); // Don't ask me why it works, it just does -- do not remove
            ObservableCollection<Pet> petList = lstPets.ItemsSource as ObservableCollection<Pet>;
            //ObservableCollection<Pet> filteredList = new ObservableCollection<Pet>();
            
            foreach (Pet pet in petList)
            {
                if (pet.Name.Equals("Peanuts") || pet.Name.Equals("Foxy") || pet.Name.Equals("Lily"))
                {
                    FavePets.Add(pet);
                }
            }
            lstPets.ItemsSource = FavePets;
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
    }
}