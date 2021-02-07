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
            BindingContext = new PetsViewModel();
            imgChoosen.Source = ImageSource.FromResource("ic_pets.png"); // NOTE - CHANGE THIS
            services = new DBFirebase();
        }


        private async void btnTakePic_Clicked(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 40, /* percentage */
            });

            if (photo != null)
                imgChoosen.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
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

        private async void btnUpload_Clicked(object sender, EventArgs e)
        {
            Busy(); // NOTE - DIDN'T GET PAST THIS POINT
            url = await services.UploadFile(file.GetStream(), Path.GetFileName(file.Path));
            NotBusy();
        }

        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            btnSelectPic.IsEnabled = false;
            btnTakePic.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            btnSelectPic.IsEnabled = true;
            btnTakePic.IsEnabled = true;
            btnUpload.IsEnabled = true;
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
             await services.AddPetTask(name.Text, Convert.ToInt32(age.Text), url);
             name.Text = string.Empty;
             age.Text = string.Empty;
             imgChoosen.Source = ImageSource.FromResource("ic_pets.png"); // NOTE - CHANGE THIS
             await DisplayAlert("Success", "Pet Added Successfully", "OK");

            // now we need to update list of pets -- check if it doesn't do it automatically
            /* var allPets = services.getPets();
             lstPets.ItemsSource = allPets; 
            */
            
        }
    }
    }

