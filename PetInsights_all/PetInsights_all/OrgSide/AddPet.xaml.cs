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
        public AddPet()
        {
            InitializeComponent();
            imgChoosen.Source = "%drawable/ic_pets.png";
            services = new DBFirebase();


            // Next Page button disabled until everything filled out -- logic still isn't thought through
            //btnAddPet.IsEnabled = false;

            // initialize the UI
            dogButton.BackgroundColor = Color.LightGray;
            catButton.BackgroundColor = Color.LightGray;
            exoticButton.BackgroundColor = Color.LightGray;
        }


        private async void btnTakePic_Clicked(object sender, EventArgs e)
        {
            // NOTE - assuming we can only upload one image here
            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 40, // percentage 
            });

            if (file != null)
                imgChoosen.Source = ImageSource.FromStream(() => { return file.GetStream(); });
        }

        private async void btnSelectPic_Clicked(object sender, EventArgs e)
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
            btnSelectPic.IsEnabled = false;
            btnTakePic.IsEnabled = false;
            //btnUpload.IsEnabled = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            btnSelectPic.IsEnabled = true;
            btnTakePic.IsEnabled = true;
            //btnUpload.IsEnabled = true;
        }

        private async void PetTypeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = dog, cat, exotic

            if ((sender as Button).BackgroundColor == Color.LightGray)
            {
                (sender as Button).BackgroundColor = Color.Blue;
                _petType = buttonName;
                if(buttonName != dogButton.BindingContext as string)
                {
                    dogButton.BackgroundColor = Color.LightGray;
                }
                if (buttonName != catButton.BindingContext as string)
                {
                    catButton.BackgroundColor = Color.LightGray;
                }
                if (buttonName != exoticButton.BindingContext as string)
                {
                    exoticButton.BackgroundColor = Color.LightGray;
                }
            }
            else if ((sender as Button).BackgroundColor == Color.Blue)
            {
                (sender as Button).BackgroundColor = Color.LightGray;
                _petType = null;
            }
        }

       

        private async void BtnNextPage_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddPet2(_petType, name.Text, Convert.ToInt32(age.Text), sex.Text, file));

            // Reset page entries
            dogButton.BackgroundColor = Color.LightGray;
            catButton.BackgroundColor = Color.LightGray;
            exoticButton.BackgroundColor = Color.LightGray;
            name.Text = string.Empty;
            age.Text = string.Empty;
            sex.Text = string.Empty;
            imgChoosen.Source = "%drawable/ic_pets.png";
        }
    }
}

