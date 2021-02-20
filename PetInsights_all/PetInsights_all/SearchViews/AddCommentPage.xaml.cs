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
            Intro.Text = "Leave a comment on your interaction with " + pet.Name;
        }

        public async void BtnPostComment(object sender, EventArgs e)
        {
            string comment = Comment.Text;
            await services.AddPetComment(pet, comment);
            await Navigation.PopAsync();
        }
        
    }
}