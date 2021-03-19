using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetInsights_all.Services;
using PetInsights_all.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all.SearchViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCommentPage : ContentPage
    {
        DBFirebase services;
        Pet pet;
        public AddCommentPage(Pet _pet)
        {
            services = new DBFirebase();
            pet = _pet;
            InitializeComponent();
            Title = "Leave a comment for " + pet.Name;
            Intro.Text = "How was your interaction with " + pet.Name+"?";
            Comment.Placeholder = pet.Name + " was energetic, loved to play, scared of other dogs, ...";
        }

        public async void BtnPostComment(object sender, EventArgs e)
        {
            string comment = Comment.Text;
            await services.AddPetComment(pet, comment);
            //await Application.Current.MainPage.Navigation.PopAsync(); // broke the nav bar maybe
            await Navigation.PopAsync();
        }
        
    }
}