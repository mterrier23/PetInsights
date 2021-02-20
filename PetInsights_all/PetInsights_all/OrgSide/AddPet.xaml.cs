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
        public AddPet()
        {
            InitializeComponent();
            //BindingContext = new PetsViewModel();
            imgChoosen.Source = ImageSource.FromResource("ic_pets.png"); // NOTE - CHANGE THIS
            services = new DBFirebase();
        }


        private async void btnTakePic_Clicked(object sender, EventArgs e)
        {
            // NOTE - testing with shared file variable (assuming we can only upload one image here)

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
                    CompressionQuality = 40, /* percentage */
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

        // NOTE: added url line into addPet -- upload image button isn't needed anymore
        /*
        private async void btnUpload_Clicked(object sender, EventArgs e)
        {
            Busy();
            url = await services.UploadFile(file.GetStream(), Path.GetFileName(file.Path));
            NotBusy();
        }
        */

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

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            Busy();
            url = await services.UploadFile(file.GetStream(), Path.GetFileName(file.Path));
            Pet p = await services.AddPetTask(name.Text, Convert.ToInt32(age.Text), url);
            
            name.Text = string.Empty;
            age.Text = string.Empty;
            imgChoosen.Source = ImageSource.FromResource("ic_pets.png"); // NOTE - CHANGE THIS
            NotBusy();
            await DisplayAlert("Success", "Pet Added Successfully", "OK");
            await Navigation.PushAsync(new PetDetailsPage(p));


            // now we need to update list of pets -- check if it doesn't do it automatically
            /* var allPets = services.getPets();
             lstPets.ItemsSource = allPets; 
            */

        }
    }
    }

