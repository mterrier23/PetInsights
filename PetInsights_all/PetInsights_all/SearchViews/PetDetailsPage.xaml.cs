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

namespace PetInsights_all.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PetDetailsPage : ContentPage
    {
        DBFirebase services;
        Pet pet;
         

        public PetDetailsPage(Pet _pet)
        {
            InitializeComponent();
            pet = _pet;
            BindingContext = _pet;
            Console.WriteLine("**pet imgsource = " + _pet.ImgIcon);
            services = new DBFirebase();
            var comments = _pet.Comments;
            var topComments = comments.Take(5).ToList();
            lstComments.ItemsSource = topComments;

            // Note - have to check user data if they've already favorited this pet or not
            faveImage.Source = "@drawable/star_empty.png";
        }

        void OnFavoritesTapped(object sender, EventArgs args)
        {
            try
            {
                Console.WriteLine("fav button tapped");
                Console.WriteLine("faveImage source = " + faveImage.Source.ToString());
                // NOTE - if this doesn't work, just base off of the user's fave table for this pet
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