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
        List<List<string>> filters;


        public PetListView(List<List<string>> _filters, bool fromMain)
        {
            if (fromMain)
                Application.Current.MainPage.Navigation.PushModalAsync(new SimpleFilterModal());

            InitializeComponent();
            BindingContext = new PetsViewModel();

            newFlag = true;


            // List<List<string>> filters = new List<List<string>>(); 
            List<List<string>> filters = _filters;

            allPets = lstPets.ItemsSource as ObservableCollection<Pet>;

            // TO DO - WORK ON THIS AFTERWARDS!
            //lstPets.ItemsSource = filterPets(filters);


            // NOTE -- have to call the filter function now and reassign the pet list !!

            MessagingCenter.Subscribe<List<List<string>>>(this, "filtersChanged", (filterSet) =>
            {
                Console.WriteLine("Message receieved");
                lstPets.ItemsSource = filterPets(filterSet);
            });

        }


        // add other filters here
        public ObservableCollection<Pet> filterPets(List<List<string>> filters)
        {
            Console.WriteLine("in filter pets");
            if(filters[0] != null)
            {
                Console.WriteLine("pet type count = " + filters[0].Count);
            }
            if(filters[1] == null)  // DIDN'T ENTER HERE .. WHY NOT
            {
                Console.WriteLine("but no genders selected");
            }
            string gender = "Both";
            ObservableCollection<Pet> filteredList = new ObservableCollection<Pet>();
            if (gender != "Both")
            {
                foreach (Pet pet in allPets)
                {
                    if (pet.Sex == gender)
                    {
                        filteredList.Add(pet);
                    }

                    // TODO - ADD THE OTHER ONES HERE !!
                }
            }
            else
            {
                filteredList = allPets; //TODO make sure to include other filters
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
            await Navigation.PushModalAsync(new FilterModal());
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App, List<List<string>>>((App)Xamarin.Forms.Application.Current, "filtersChanged");
            GC.Collect();
        }
    }
}