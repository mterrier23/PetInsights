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
            allPets = petList;

            // Page UI components:
            BindingContext = pet;
            Title = pet.Name + " - " + pet.Affiliation;
            shelteredWithLabel.Text = "See who else is sheltered with " + pet.Name + "...";
            var comments = pet.Comments;
            var topComments = comments.Take(5).ToList();    // limits to only the top five comments
            lstComments.ItemsSource = topComments;

            // TODO - have to check user data if they've already favorited this pet or not
            faveImage.Source = "@drawable/star_empty.png";

            // Get filtered list of pets for shared affiliation
            services = new DBFirebase();
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
            Console.WriteLine("shared pet clicked = "+tpet.Name);

            if (tpet == null)
            {
                Console.WriteLine("Pet is Null");
                return;
            }

            // NOTE maybe need to do a PopAsync first
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
            //await Navigation.PopAsync();
        }
    }
}