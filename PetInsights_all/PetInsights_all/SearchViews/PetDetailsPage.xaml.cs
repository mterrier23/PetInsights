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