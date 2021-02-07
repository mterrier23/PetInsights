using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetInsights_all.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirebaseTutorial : ContentPage
    {
        FirebaseHelper_old firebaseHelper = new FirebaseHelper_old();
        public FirebaseTutorial()
        {
           // InitializeComponent();
        }

        // Future useful code for adding and updating pet info
        

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
        /*
            await firebaseHelper.AddPet(Convert.ToInt32(txtId.Text), txtName.Text);
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            await DisplayAlert("Success", "Pet Added Successfully", "OK");
            var allPets = await firebaseHelper.GetAllPets();
            lstPets.ItemsSource = allPets;
        */
        }

        private async void BtnRetrive_Clicked(object sender, EventArgs e)
        {
            /*
            var pet = await firebaseHelper.GetPet(Convert.ToInt32(txtId.Text));
            if (pet != null)
            {
                txtId.Text = pet.PetId.ToString();
                txtName.Text = pet.Name;
                await DisplayAlert("Success", "Pet Retrive Successfully", "OK");

            }
            else
            {
                await DisplayAlert("Success", "No Pet Available", "OK");
            }
            */

        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            /*
            await firebaseHelper.UpdatePet(Convert.ToInt32(txtId.Text), txtName.Text);
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            await DisplayAlert("Success", "Pet Updated Successfully", "OK");
            var allPets = await firebaseHelper.GetAllPets();
            lstPets.ItemsSource = allPets;
            */
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            /*
            await firebaseHelper.DeletePet(Convert.ToInt32(txtId.Text));
            await DisplayAlert("Success", "Pet Deleted Successfully", "OK");
            var allPets = await firebaseHelper.GetAllPets();
            lstPets.ItemsSource = allPets;
            */
        }
        
    }
}