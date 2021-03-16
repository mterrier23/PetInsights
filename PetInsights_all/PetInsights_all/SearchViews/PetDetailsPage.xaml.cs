using PetInsights_all.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetInsights_all.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PetInsights_all.SearchViews;
using PetInsights_all.ViewModel;
using System.Collections.ObjectModel;

namespace PetInsights_all.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PetDetailsPage : ContentPage
    {
        DBFirebase services;
        Pet pet;
        ObservableCollection<Pet> allPets;

        public PetDetailsPage(Pet _pet, ObservableCollection<Pet> petList)
        {     
            InitializeComponent();

            pet = _pet;
            if (petList != null)
            {
                allPets = petList;
            }
            else
            {   
                // Came from AddPet logic, so need to remove the two "Add Pet Details" pages
                for (var counter = 0; counter < 2; counter++)
                {
                    Application.Current.MainPage.Navigation.RemovePage(Application.Current.MainPage.Navigation.NavigationStack[Application.Current.MainPage.Navigation.NavigationStack.Count - 1]);
                }
            }



            // Page UI components:
            BindingContext = pet;
            Title = pet.Name + " - " + pet.Affiliation;
            shelteredWithLabel.Text = "See who else is sheltered with " + pet.Name + "...";
           // var comments = pet.Comments;
            //var topComments = comments.Take(6).ToList();    // limits to only the top five comments
            //topComments.RemoveAt(0);    // bc first is blank
            //lstComments.ItemsSource = topComments;

            if (pet.Comments.Count == 1 && pet.Comments[0].Equals(""))
            {
                Console.WriteLine("comments count  = 1 and is blank");
                hasNoComments.IsVisible = true;
                lstComments.IsVisible = false;
                SeeComments.IsVisible = false;
            }
            else
            {
                hasNoComments.IsVisible = false;
                lstComments.IsVisible = true;
                if (pet.Comments.Count > 4)
                    SeeComments.IsVisible = true;
                else
                    SeeComments.IsVisible = false;
                lstComments.ItemsSource = pet.Comments;
            }
            // NOTE - isENabled = false makes it not scrollable (but not sure what our desired behavior is)
            //else lstComments.ItemsSource = pet.Comments; // size wise this is fine, but need to remove the first blank!!

            // TODO - have to check user data if they've already favorited this pet or not
            faveImage.Source = "@drawable/star_empty.png";

            // Get filtered list of pets for shared affiliation
            services = new DBFirebase();
            if (petList != null)
                lstSharedPets.ItemsSource = services.GetAffiliatedPets(petList, pet);
        }

        void OnFavoritesTapped(object sender, EventArgs args)
        {
            try
            {
                // NOTE - once user table works, base these values off of user's preference and re-assign in DB each time
                if (faveImage.Source.ToString() == "File: @drawable/star_empty.png")
                    faveImage.Source = "@drawable/star_filled.png";
                else if (faveImage.Source.ToString() == "File: @drawable/star_filled.png")
                    faveImage.Source = "@drawable/star_empty.png";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async void lstPets_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            var tpet = e.CurrentSelection.First() as Pet;

            if (tpet == null)
            {
                Console.WriteLine("Pet is Null");
                return;
            }

            await Navigation.PushAsync(new PetDetailsPage(tpet, allPets));

        }


        public async void BtnUpdate_Pet(object sender, EventArgs e)
        {
            /*
            await services.UpdatePet(
                Name.Text, int.Parse(Age.Text));
            await Navigation.PopAsync();
            */
        }

        public async void BtnAddComment(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCommentPage(pet));
            //await Navigation.PopAsync();
        }

        public async void BtnAddMedia(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddMediaPage(pet));
            //await Navigation.PopAsync();
        }

        public async void BtnSeeComments(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PetCommentsPage(pet));
        }

    }
}