using PetInsights_all.Model;
using PetInsights_all.Services;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all.SearchViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMediaPage : ContentPage
    {
        DBFirebase service;
        List<Stream> _imgStreams;
        List<string> urls;
        Pet pet;
        public AddMediaPage(Pet _pet)
        {
            InitializeComponent();
            Title = "Select photos of " + _pet.Name;
            pet = _pet;
            service = new DBFirebase();
            _imgStreams = new List<Stream>();
            urls = new List<string>();
            Device.BeginInvokeOnMainThread(async () => await AskForPermissions());
        }
        
        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
        {
            //Check users permissions.
            var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
            if (storagePermissions == PermissionStatus.Granted && photoPermissions == PermissionStatus.Granted)
            {
                //If we are on iOS, call GMMultiImagePicker.
                if (Device.RuntimePlatform == Device.iOS)
                {
                    //If the image is modified (drawings, etc) by the users, you will need to change the delivery mode to HighQualityFormat.
                    bool imageModifiedWithDrawings = false;
                    if (imageModifiedWithDrawings)
                    {
                        await GMMultiImagePicker.Current.PickMultiImage(true);
                    }
                    else
                    {
                        await GMMultiImagePicker.Current.PickMultiImage();
                    }

                    MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
                    MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS", (s, images) =>
                    {
                        //If we have selected images, put them into the carousel view.
                        if (images.Count > 0)
                        {
                            ImgCarouselView.ItemsSource = images;   // NOTE -- ideally have them compressed before we reach this point!!
                            InfoText.IsVisible = true; //InfoText is optional
                        }
                    });
                }
                //If we are on Android, call IMediaService.
                else if (Device.RuntimePlatform == Device.Android)
                {
                    DependencyService.Get<IMediaService>().OpenGallery();

                    MessagingCenter.Subscribe<List<string>>(this, "ImagesSelectedAndroid", (images) => 
                    {
                        //If we have selected images, put them into the carousel view.
                        if (images.Count > 0)
                        {
                            ImgCarouselView.ItemsSource = images;   // NOTE -- ideally have them compressed before reaching this point !!
                            InfoText.IsVisible = true; //InfoText is optional
                        }
                    });

                    MessagingCenter.Subscribe<List<Stream>>(this, "ImagesStreamAndroid", (imgStream) =>
                    {
                        //If we have selected images, put them into the carousel view.
                        if (imgStream.Count > 0)
                        {
                            _imgStreams = imgStream;
                        }
                    });

                }
            }
            else
            {
                await DisplayAlert("Permission Denied!", "\nPlease go to your app settings and enable permissions.", "Ok");
            }
        }

        
        private async void UploadImagesButton_Clicked(object sender, EventArgs e)
        {
            // Get the list of images we have selected.
            List<string> imagePaths = ImgCarouselView.ItemsSource as List<string>;

            // If user is using Android, compress the images. (Optional)
            if (Device.RuntimePlatform == Device.Android)
            {
                imagePaths = CompressAllImages(imagePaths);
            }      

            // Change info text. (Optional)
            InfoText.Text = "Uploading...";

            for (int i = 0; i < imagePaths.Count; i++)
            {
                System.IO.Stream str = _imgStreams[i];
                string path = Path.GetFileName(imagePaths[i]) + ".jpg";
                string url = (await service.UploadFile(str, path));
                urls.Add(url);
            }

            await service.AddPetMedia(pet, urls);

            // Change info text. (Optional)
            InfoText.Text = "Upload complete.";

            // Clear temp files after upload.
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IMediaService>().ClearFileDirectory();
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                GMMultiImagePicker.Current.ClearFileDirectory();
            }
            //await Application.Current.MainPage.Navigation.PopAsync(); // broke the nav bar maybe
            await Navigation.PopAsync();
        }

        /// <summary>
        ///     Compress Android images before uploading them to Azure Blob Storage.
        /// </summary>
        /// <param name="totalImages"></param>
        /// <returns></returns>
        private List<string> CompressAllImages(List<string> totalImages)
        {
            int displayCount = 1;
            int totalCount = totalImages.Count;
            List<string> compressedImages = new List<string>();
            foreach (string path in totalImages)
            {
                string newpath = DependencyService.Get<ICompressImages>().CompressImage(path);
                compressedImages.Add(newpath);
                displayCount++;
            }
            return compressedImages;
        }

        

        /// <summary>
        ///     Make sure Permissions are given to the users storage.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> AskForPermissions()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage, Permission.Photos });
                    storagePermissions = results[Permission.Storage];
                    photoPermissions = results[Permission.Photos];
                }

                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permissions Denied!", "Please go to your app settings and enable permissions.", "Ok");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error. permissions not set. here is the stacktrace: \n" + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        ///     Unsubsribe from the MessagingCenter on disappearing.
        /// </summary>
        
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
            MessagingCenter.Unsubscribe<App, List<Stream>>((App)Xamarin.Forms.Application.Current, "ImagesStreamAndroid");
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
            GC.Collect();
        }
        
    }
}