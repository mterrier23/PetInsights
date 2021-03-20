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
        string _medCond;
        string _pottyTrained;
        string _apartmentFriendly;
        MediaFile file;



        public AddPet2(string _petType, string _name, int _age, string _sex, MediaFile _file)
        {
            InitializeComponent();
            services = new DBFirebase();
            petName.Text = "Tell us a little bit about "+_name+"'s personality ";

            // Pass over previous page's info
            petType = _petType;
            name = _name;
            age = _age;
            sex = _sex;
            file = _file;

            // initialize the button colors
            smallBtn.BackgroundColor = Color.White;
            smallBtn.BorderColor = Color.LightGray;
            smallBtn.TextColor = Color.Black;


            mediumBtn.BackgroundColor = Color.White;
            mediumBtn.BorderColor = Color.LightGray;
            mediumBtn.TextColor = Color.Black;

            largeBtn.BackgroundColor = Color.White;
            largeBtn.BorderColor = Color.LightGray;
            largeBtn.TextColor = Color.Black;

            yesMedBtn.BackgroundColor = Color.White;
            yesMedBtn.BorderColor = Color.LightGray;
            yesMedBtn.TextColor = Color.Black;

            noMedBtn.BackgroundColor = Color.White;
            noMedBtn.BorderColor = Color.LightGray;
            noMedBtn.TextColor = Color.Black;

            medicalConditionDetails.IsVisible = false;
            medicalConditionDetails.HeightRequest = 0;

            pottyTrained.BackgroundColor = Color.White;
            pottyTrained.BorderColor = Color.LightGray;
            pottyTrained.TextColor = Color.Black;

            apartmentFriendly.BackgroundColor = Color.White;
            apartmentFriendly.BorderColor = Color.LightGray;
            apartmentFriendly.TextColor = Color.Black;
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
                _medCond,
                medicalConditionDetails.Text,
                personality.Text,
                _pottyTrained,
                _apartmentFriendly
                ); ;

            NotBusy();

            // TODO - prettify this alert
            var result = await DisplayAlert(null,
                        "Pet added successfully!",
                        "Add another", "View my pets");

            // true for view my pets, false for add another
            if (!result)
            {
                // View my Pets
                await Application.Current.MainPage.Navigation.PushAsync(new PetDetailsPage(p, null));
            }
            if (result)
            {
                // Add another pet
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            //await Application.Current.MainPage.Navigation.PushAsync(new PetDetailsPage(p, null)); 

            // now we need to update list of pets -- check if it doesn't do it automatically
            /* var allPets = services.getPets();
             lstPets.ItemsSource = allPets; 
            */

        }

        private async void PetSizeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = small, medium, large

            if ((sender as Button).BackgroundColor == Color.White)
            {
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
                _petSize = buttonName;
                if (buttonName != smallBtn.BindingContext as string)
                {
                    smallBtn.BackgroundColor = Color.White;
                    smallBtn.TextColor = Color.Black;
                    smallBtn.BorderColor = Color.LightGray;
                }
                if (buttonName != mediumBtn.BindingContext as string)
                {
                    mediumBtn.BackgroundColor = Color.White;
                    mediumBtn.TextColor = Color.Black;
                    mediumBtn.BorderColor = Color.LightGray;
                }
                if (buttonName != largeBtn.BindingContext as string)
                {
                    largeBtn.BackgroundColor = Color.White;
                    largeBtn.TextColor = Color.Black;
                    largeBtn.BorderColor = Color.LightGray;

                }
            }
            else if ((sender as Button).BackgroundColor == Color.Orange)
            {
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
                _petSize = null;
            }
        }

        private async void PetMedBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = yes, no
            if (buttonName.Equals("yes"))
            {
                medicalConditionDetails.IsVisible = true;
                medicalConditionDetails.HeightRequest = 50;
            }

            if ((sender as Button).BackgroundColor == Color.White)
            {
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
                _medCond = buttonName;
                if (buttonName != yesMedBtn.BindingContext as string)
                {
                    yesMedBtn.BackgroundColor = Color.White;
                    yesMedBtn.TextColor = Color.Black;
                    yesMedBtn.BorderColor = Color.LightGray;
                }
                else if (buttonName != noMedBtn.BindingContext as string)
                {
                    noMedBtn.BackgroundColor = Color.White;
                    noMedBtn.TextColor = Color.Black;
                    noMedBtn.BorderColor = Color.LightGray;
                }
            }
            else if ((sender as Button).BackgroundColor == Color.Orange)
            {
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
                _medCond = null;
            }
        }


        private async void PottyTrainedBtnClicked(object sender, EventArgs e)
        {

            if ((sender as Button).BackgroundColor == Color.White)
            {
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
                _pottyTrained = "yes";
            }
            else if ((sender as Button).BackgroundColor == Color.Orange)
            {
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
                _pottyTrained = "no";
            }
        }

        
        private async void ApartmentFriendlyClicked(object sender, EventArgs e)
        {

            if ((sender as Button).BackgroundColor == Color.White)
            {
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
                _apartmentFriendly = "yes";
            }
            else if ((sender as Button).BackgroundColor == Color.Orange)
            {
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
                _apartmentFriendly = "no";
            }
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