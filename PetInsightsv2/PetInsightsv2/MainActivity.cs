using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android;
using Plugin.Media;
using Android.Graphics;
using Firebase;
using Firebase.Storage;
using Android.Gms.Tasks;
using FFImageLoading;

namespace PetInsightsv2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnSuccessListener, IOnFailureListener
    {

        Button captureButton;
        Button selectButton;
        Button uploadButton;
        Button downloadButton;

        ImageView thisImageView;
        private byte[] imageArray;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        StorageReference storageReference;
        private string downloadUrl;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            captureButton = (Button)FindViewById(Resource.Id.captureButton);
            selectButton = (Button)FindViewById(Resource.Id.selectButton);
            uploadButton = (Button)FindViewById(Resource.Id.uploadButton);
            downloadButton = (Button)FindViewById(Resource.Id.downloadButton);
            thisImageView = (ImageView)FindViewById(Resource.Id.thisImageView);

            captureButton.Click += CaptureButton_Click;
            selectButton.Click += SelectButton_Click;
            uploadButton.Click += UploadButton_Click;
            downloadButton.Click += DownloadButton_Click;
            RequestPermissions(permissionGroup, 0);
            InitializeFirebase();

        }

        void InitializeFirebase()
        {
            var app = FirebaseApp.InitializeApp(this);
            if(app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("petinsights-4dc78")
                    .SetApplicationId("petinsights-4dc78")
                    .SetApiKey("AIzaSyDGQ7TwHNR9Ov-K0dXGgW5Fhq5Xqz31vHU")
                    .SetDatabaseUrl("https://petinsights-4dc78.firebaseio.com")
                    .SetStorageBucket("petinsights-4dc78.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(this, options);

            }
        }

        private void SelectButton_Click(object sender, System.EventArgs e)
        {
            SelectPhoto();
        }

        private void DownloadButton_Click(object sender, System.EventArgs e)
        {
            ImageService.Instance.LoadUrl("https://firebasestorage.googleapis.com/v0/b/petinsights-4dc78.appspot.com/o/postImages%2FsecondImage?alt=media&token=e1e5b3fb-8851-4e42-a9e1-becbc309b89a")
                // rn it's hardcoded in the "" but this will be a sql call to get it
                .Retry(3, 200) // helpful if there's bad internet - retry 3 times and wait 200 ms bw
                .DownSample(500, 500) // if the image is greater than 500 by 500 we downsize it
                .Into(thisImageView);
        }
        private void UploadButton_Click(object sender, System.EventArgs e)
        {
            // convert images to byte array then send that to the firebase storage
            if(imageArray != null)
            {
                // if we want to save each image in a particular folder, this is where we do it
                storageReference = FirebaseStorage.Instance.GetReference("postImages/secondImage"); // in tutorial, changed this from firstImage
                // postImages is the folder we want it in
                // firstImage is the name we're assigning to the file we're uploading
                storageReference.PutBytes(imageArray)
                    .AddOnSuccessListener(this)
                    .AddOnFailureListener(this);
                // to know if our upload was successful or not ^^^
            }
        }

        private void CaptureButton_Click(object sender, EventArgs e)
        {
            TakePhoto();   
        }

        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            // waits to run photo-taking + compresses it once it's taken
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40, /* percentage */
                Name = "myimage.jpg",
                Directory = "sample"
            });

            if(file == null)
            {
                return;
            }

            // once image is successfully captured:
            // Convert file to byte array and set the resulting bitmap to imageview
            imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            thisImageView.SetImageBitmap(bitmap);
        }

        async void SelectPhoto()
        {
            await CrossMedia.Current.Initialize();

            if(!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Upload not supported on this device", ToastLength.Short).Show();
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 40

            });

            // Convert file to byre array, to bitmap and set it to our ImageView
            imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            thisImageView.SetImageBitmap(bitmap);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            if(storageReference != null)
            {
                Toast.MakeText(this, "Successful Upload", ToastLength.Short).Show();
                // goes here once we successfully upload an image (we'll be able to access download url from here)
                storageReference.GetDownloadUrl()
                    .AddOnSuccessListener(this); // we need to implement another one
                    // ^ this interface shares the same OnSuccess method
            }

            if(!string.IsNullOrEmpty(result.ToString()))
            {
                downloadUrl = result.ToString();
                Toast.MakeText(this, downloadUrl, ToastLength.Short).Show();
                // NOTE - a toast is a message that pops up on the screen

                System.Console.WriteLine("Download Url = " + downloadUrl);
                // SAVE THIS DOWNLOAD URL INTO THE DB TO ASSOCIATE IT WITH THE IMAGE !!
            }
           
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            
        }
    }
}