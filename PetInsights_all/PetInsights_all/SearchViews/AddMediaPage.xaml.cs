using PetInsights_all.Services;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all.SearchViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMediaPage : ContentPage
    {
        List<string> _images;
        public AddMediaPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", (s, images) =>
            {
                _images = images;
            });
            Device.BeginInvokeOnMainThread(async () => await AskForPermissions());
        }
        
        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("**SelectImages clicked");
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
                            _images = images;
                            ImgCarouselView.ItemsSource = images;
                            InfoText.IsVisible = true; //InfoText is optional
                        }
                    });
                }
                //If we are on Android, call IMediaService.
                else if (Device.RuntimePlatform == Device.Android)
                {
                    Console.WriteLine("***Knows device is android");
                    DependencyService.Get<IMediaService>().OpenGallery();

                    //MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
                   // MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", async (s, images) =>
                    MessagingCenter.Subscribe<List<string>>(this, "ImagesSelectedAndroid", (images) => 
                    {
                        //If we have selected images, put them into the carousel view.
                        Console.WriteLine("***image count= " + images.Count);
                        if (images.Count > 0)
                        {
                            _images = images;
                            ImgCarouselView.ItemsSource = images;
                            InfoText.IsVisible = true; //InfoText is optional
                        }
                    });
                    //MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
                    Console.WriteLine("**end of function");
                }
            }
            else
            {
                await DisplayAlert("Permission Denied!", "\nPlease go to your app settings and enable permissions.", "Ok");
            }
            //Console.WriteLine("***_image count= " + _images.Count); // null pointer
            //ImgCarouselView.ItemsSource = _images;
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

            Console.WriteLine("***In the upload button");
            // url = await services.UploadFile(file.GetStream(), Path.GetFileName(file.Path));


          

            // Change info text. (Optional)
            InfoText.Text = "Uploading to Azure Blob Storage...";

            // Upload all the selected images to Azure Blob Storage.
            int count = 1;
            foreach (string img in imagePaths)
            {
                // Retrieve reference to a blob named "newphoto#.jpg".
               /* CloudBlockBlob blockBlob = container.GetBlockBlobReference("newphoto" + count + ".jpg");

                // Create the "newphoto#.jpg" blob with the current image in our list.
                await blockBlob.UploadFromFileAsync(img);
               */

                count++;
            }

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
                compressedImages.Add(DependencyService.Get<ICompressImages>().CompressImage(path));
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
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
            GC.Collect();
        }
        
    }
}