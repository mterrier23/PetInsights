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
using PetInsights_all.SearchViews;

namespace PetInsights_all

{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PetListView : ContentPage
    {
        ObservableCollection<Pet> allPets = new ObservableCollection<Pet>();
        bool newFlag;
        int check = 0;


        public PetListView()
        {
            InitializeComponent();
            BindingContext = new PetsViewModel();
            newFlag = true;
            //ObservableCollection<Pet> allPets = lstPets.ItemsSource as ObservableCollection<Pet>;


            MessagingCenter.Subscribe<FilterModal,string>(this, "selectionChanged", (sender, genderflag) =>
            {
                Console.WriteLine("The MESSAGE WORKED!");
                lstPets.ItemsSource = filterPets(genderflag);
            });
            
        }



        public ObservableCollection<Pet> filterPets(string gender)
        {
            ObservableCollection<Pet> filteredList = new ObservableCollection<Pet>();
            foreach (Pet pet in allPets)
            {
                if (pet.Sex == gender)
                {
                    filteredList.Add(pet);
                }
            }
            return filteredList;

        }


        async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void OnFilterButtonClicked(object sender, EventArgs e)
        {
            if (newFlag == true)
            {
                allPets = lstPets.ItemsSource as ObservableCollection<Pet>;
                newFlag = false;
            }
            await Navigation.PushModalAsync(new FilterModal(allPets));
        }


        // NOTE - not being used, could be helpful elsewhere though
        public async void OnTapGesture(object sender, EventArgs e)
        {
            Console.WriteLine("Tap Success!");
            await Navigation.PopAsync();
            //await Navigation.PushAsync(new PetDetailsPage(pet))
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