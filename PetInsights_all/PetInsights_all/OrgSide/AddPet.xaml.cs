using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Storage;
using System.Diagnostics;
using System.IO;
using PetInsights_all.Services;
using PetInsights_all.ViewModel;
using PetInsights_all.Model;
using PetInsights_all.Search;

namespace PetInsights_all.OrgSide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPet : ContentPage
    {
        private DBFirebase services;
        MediaFile file;
        string url;
        string _petType;
        string _petSex;
        public AddPet()
        {
            InitializeComponent();
            imgChoosen.Source = null;
            services = new DBFirebase();


            // Next Page button disabled until everything filled out -- logic still isn't thought through
            //btnAddPet.IsEnabled = false;

            // initialize the UI
            dogButton.BackgroundColor = Color.White;
            dogButton.BorderColor = Color.LightGray;
            dogButton.TextColor = Color.Black;

            catButton.BackgroundColor = Color.White;
            catButton.BorderColor = Color.LightGray;
            catButton.TextColor = Color.Black;

            exoticButton.BackgroundColor = Color.White;
            exoticButton.BorderColor = Color.LightGray;
            exoticButton.TextColor = Color.Black;


            maleButton.BackgroundColor = Color.White;
            maleButton.BorderColor = Color.LightGray;
            maleButton.TextColor = Color.Black;

            femaleButton.BackgroundColor = Color.White;
            femaleButton.BorderColor = Color.LightGray;
            femaleButton.TextColor = Color.Black;

            imgChoosen.HeightRequest = 0;

        }


        private async void btnTakePic_Clicked()
        {
            // NOTE - assuming we can only upload one image here
            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 40, // percentage 
            });

            if (file != null)
            {
                imgChoosen.HeightRequest = 150;
                imgChoosen.Source = ImageSource.FromStream(() => { return file.GetStream(); });
            }
        }

        private async void btnPic_Clicked(object sender, EventArgs e)
        {
              var result = await DisplayAlert(null,
                        "How would you like to add a photo?",
                        "Take Photo", "Upload from Camera Roll");

            // true for view my pets, false for add another
            if (!result)
            {
                // Upload Photo
                btnSelectPic_Clicked();
            }
            if (result)
            {
                // Take Photo
                btnTakePic_Clicked();
            }

        }

        private async void btnSelectPic_Clicked()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 40, // percentage 
                });
                if (file == null)
                    return;
                imgChoosen.HeightRequest = 150;
                imgChoosen.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            //btnSelectPic.IsEnabled = false;
            //btnTakePic.IsEnabled = false;
            //btnUpload.IsEnabled = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            //btnSelectPic.IsEnabled = true;
            //btnTakePic.IsEnabled = true;
            //btnUpload.IsEnabled = true;
        }

        private async void PetTypeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = dog, cat, exotic

            if ((sender as Button).BackgroundColor == Color.White)
            {
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
                _petType = buttonName;
                if(buttonName != dogButton.BindingContext as string)
                {
                    dogButton.BackgroundColor = Color.White;
                    dogButton.TextColor = Color.Black;
                    dogButton.BorderColor = Color.LightGray;
                }
                if (buttonName != catButton.BindingContext as string)
                {
                    catButton.BackgroundColor = Color.White;
                    catButton.TextColor = Color.Black;
                    catButton.BorderColor = Color.LightGray;
                }
                if (buttonName != exoticButton.BindingContext as string)
                {
                    exoticButton.BackgroundColor = Color.White;
                    exoticButton.TextColor = Color.Black;
                    exoticButton.BorderColor = Color.LightGray;
                }
            }
            else if ((sender as Button).BackgroundColor == Color.Orange)
            {
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
                _petType = null;
            }
        }


        private async void PetSexBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = dog, cat, exotic

            if ((sender as Button).BackgroundColor == Color.White)
            {
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
                _petSex = buttonName;
                if (buttonName != maleButton.BindingContext as string)
                {
                    maleButton.BackgroundColor = Color.White;
                    maleButton.TextColor = Color.Black;
                    maleButton.BorderColor = Color.LightGray;
                }
                else if (buttonName != femaleButton.BindingContext as string)
                {
                    femaleButton.BackgroundColor = Color.White;
                    femaleButton.TextColor = Color.Black;
                    femaleButton.BorderColor = Color.LightGray;
                }
            }
            else if ((sender as Button).BackgroundColor == Color.Orange)
            {
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
                _petSex = null;
            }
        }


        private async void BtnNextPage_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddPet2(_petType, name.Text, Convert.ToInt32(age.Text), _petSex, file));

            // Reset page entries
            dogButton.BackgroundColor = Color.White;
            catButton.BackgroundColor = Color.White;
            exoticButton.BackgroundColor = Color.White;
            name.Text = string.Empty;
            age.Text = string.Empty;
            femaleButton.BackgroundColor = Color.White;
            maleButton.BackgroundColor = Color.White;
            imgChoosen.Source = null;
        }
    }
}

