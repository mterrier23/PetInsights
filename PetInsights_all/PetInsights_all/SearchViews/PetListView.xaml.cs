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
                Application.Current.MainPage.Navigation.PushAsync(new SimpleFilterModal());

            InitializeComponent();

            showPets.IsVisible = true;
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

        public PetListView(string filter)
        {
            
            InitializeComponent();
            showPets.IsVisible = false;
            BindingContext = new PetsViewModel();

            filterQuickPets(filter);
            //lstPets.ItemsSource gets reassigned within the above method


            newFlag = true;

            MessagingCenter.Subscribe<List<List<string>>>(this, "filtersChanged", (filterSet) =>
            {
                Console.WriteLine("Message receieved");
                lstPets.ItemsSource = filterPets(filterSet);
            });

        }

        public async void filterQuickPets(string filter)
        {

            ObservableCollection<Pet> filteredList = new ObservableCollection<Pet>();
            await Task.Delay(1500);
            ObservableCollection<Pet> thepets = lstPets.ItemsSource as ObservableCollection<Pet>;
            if (filter == "catsQuick")
            {
                foreach (Pet pet in thepets)
                {
                    if (pet.PetType == "cat")
                    {
                        filteredList.Add(pet);
                    }
                }
            }
            else if (filter == "smallDogsQuick")
            {
                foreach (Pet pet in thepets)
                {
                    if (pet.PetType == "dog" && pet.Size == "small")
                    {
                        filteredList.Add(pet);
                    }
                }
            }
            else if (filter == "mediumDogsQuick")
            {
                foreach (Pet pet in thepets)
                {
                    if (pet.PetType == "dog" && pet.Size == "medium")
                    {
                        filteredList.Add(pet);
                    }
                }
            }
            else if (filter == "largeDogsQuick")
            {
                foreach (Pet pet in thepets)
                {
                    if (pet.PetType == "dog" && pet.Size == "large")
                    {
                        filteredList.Add(pet);
                    }
                }
            }
            else if (filter == "exoticQuick")
            {
                foreach (Pet pet in thepets)
                {
                    if (pet.PetType == "exotic")
                    {
                        filteredList.Add(pet);
                    }
                }
            }
            else if (filter == "chillQuick")
            {
                foreach (Pet pet in thepets)
                {
                    if (pet.Sex == "Female")
                    {
                        filteredList.Add(pet);
                    }
                }
            }
            else if (filter == "energyQuick")
            {
                foreach (Pet pet in thepets)
                {
                    if (pet.Sex == "Male")
                    {
                        filteredList.Add(pet);
                    }
                }
            }
            else if (filter == "longQuick")
            {
                foreach (Pet pet in thepets)
                {
                    if (pet.Name == "Lily" || pet.Name == "Sheryl" || pet.Name == "Foxy")
                    {
                        filteredList.Add(pet);
                    }
                }
            }

            showPets.IsVisible = true;
            lstPets.ItemsSource = filteredList;
        }

        // add other filters here
        public ObservableCollection<Pet> filterPets(List<List<string>> filters)
        {
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
            Console.WriteLine("ListView petlist count = " + petList.Count);
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