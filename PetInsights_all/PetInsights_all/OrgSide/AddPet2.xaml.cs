using PetInsights_all.Model;
using PetInsights_all.Search;
using PetInsights_all.Services;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all.OrgSide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPet2 : ContentPage
    {
        // Page 2 Concerns Optional details

        private DBFirebase services;
        string url;

        string petType;
        string name;
        int age;
        string sex;
        string _petSize;
        MediaFile file;



        public AddPet2(string _petType, string _name, int _age, string _sex, MediaFile _file)
        {
            InitializeComponent();
            services = new DBFirebase();

            // Pass over previous page's info
            petType = _petType;
            name = _name;
            age = _age;
            sex = _sex;
            file = _file;

            // initialize the button colors
            smallBtn.BackgroundColor = Color.LightGray;
            mediumBtn.BackgroundColor = Color.LightGray;
            largeBtn.BackgroundColor = Color.LightGray;
        }


        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            Busy();
            url = await services.UploadFile(file.GetStream(), Path.GetFileName(file.Path));
            Pet p = await services.AddPetTask(
                petType,
                name,
                age,
                sex,
                url,
                breed.Text,
                _petSize,
                medicalCondition.Text,
                medicalConditionDetails.Text,
                personality.Text,
                pottyTrained.Text,
                apartmentFriendly.Text
                ); ;

            NotBusy();
            await DisplayAlert("Success", "Pet Added Successfully", "OK"); 

            await Navigation.PushAsync(new PetDetailsPage(p));

            // now we need to update list of pets -- check if it doesn't do it automatically
            /* var allPets = services.getPets();
             lstPets.ItemsSource = allPets; 
            */

        }

        private async void PetSizeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = small, medium, large

            if ((sender as Button).BackgroundColor == Color.LightGray)
            {
                (sender as Button).BackgroundColor = Color.Blue;
                _petSize = buttonName;
                if (buttonName != smallBtn.BindingContext as string)
                {
                    smallBtn.BackgroundColor = Color.LightGray;
                }
                if (buttonName != mediumBtn.BindingContext as string)
                {
                    mediumBtn.BackgroundColor = Color.LightGray;
                }
                if (buttonName != largeBtn.BindingContext as string)
                {
                    largeBtn.BackgroundColor = Color.LightGray;
                }
            }
            else if ((sender as Button).BackgroundColor == Color.Blue)
            {
                (sender as Button).BackgroundColor = Color.LightGray;
                _petSize = null;
            }
            Console.WriteLine("**Current pet size == " + _petSize);
        }



        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            //btnUpload.IsEnabled = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            //btnUpload.IsEnabled = true;
        }
    }
}