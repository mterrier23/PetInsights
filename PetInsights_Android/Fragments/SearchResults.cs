using Android;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.Media;
using Android.Graphics;
using Firebase;
using Firebase.Storage;
using Android.Gms.Tasks;
using FFImageLoading;

namespace PetInsightsv2.Fragments
{
    public class SearchResults : Fragment, IOnSuccessListener, IOnFailureListener
    {
        Context thiscontext;
        // testing
        MainActivity activity;

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

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here

            RequestPermissions(permissionGroup, 0);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            thiscontext = container.Context;
            // Use this to return your custom view for this Fragment

            View view = inflater.Inflate(Resource.Layout.add_content, container, false);

            captureButton = view.FindViewById<Button>(Resource.Id.captureButton);
            selectButton = view.FindViewById<Button>(Resource.Id.selectButton);

            uploadButton = view.FindViewById<Button>(Resource.Id.uploadButton);
            downloadButton = view.FindViewById<Button>(Resource.Id.downloadButton);
            thisImageView = view.FindViewById<ImageView>(Resource.Id.thisImageView);


            InitializeFirebase();

            captureButton.Click += CaptureButton_Click;
            selectButton.Click += SelectButton_Click;
            uploadButton.Click += UploadButton_Click;
            downloadButton.Click += DownloadButton_Click;
          //  RequestPermissions(permissionGroup, 0);


            return view;
        }

        void InitializeFirebase()
        {
            var app = FirebaseApp.InitializeApp(thiscontext);
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("petinsights-4dc78")
                    .SetApplicationId("petinsights-4dc78")
                    .SetApiKey("AIzaSyDGQ7TwHNR9Ov-K0dXGgW5Fhq5Xqz31vHU")
                    .SetDatabaseUrl("https://petinsights-4dc78.firebaseio.com")
                    .SetStorageBucket("petinsights-4dc78.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(thiscontext, options);

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
            if (imageArray != null)
            {
                // if we want to save each image in a particular folder, this is where we do it
                storageReference = FirebaseStorage.Instance.GetReference("postImages/secondImage"); // in tutorial, changed this from firstImage
                // postImages is the folder we want it in
                // firstImage is the name we're assigning to the file we're uploading
                // *** NOTE -- at this point, StorageReference is NOT null
                storageReference.PutBytes(imageArray)
                    .AddOnSuccessListener((IOnSuccessListener)activity)
                    .AddOnFailureListener((IOnFailureListener)activity);

                // ***** ERROR  **** null reference here !! TO FIX !!
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

            if (file == null)
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

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(thiscontext, "Upload not supported on this device", ToastLength.Short).Show();
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
            Console.WriteLine("***ENTER ONSUCCESS ****");
            if (storageReference != null)
            {
                Toast.MakeText(thiscontext, "Successful Upload", ToastLength.Short).Show();
                // goes here once we successfully upload an image (we'll be able to access download url from here)
                  storageReference.GetDownloadUrl()
                    .AddOnSuccessListener(this); // we need to implement another one
                                                 // ^ this interface shares the same OnSuccess method
                Console.WriteLine("***END OF ONSUCCESS FIRST IF ****");
            }

            if (!string.IsNullOrEmpty(result.ToString()))
            {
                Console.WriteLine("***ENTER ONSUCCESS SECOND IF ****");
                downloadUrl = result.ToString();
                Toast.MakeText(thiscontext, downloadUrl, ToastLength.Short).Show();
                // NOTE - a toast is a message that pops up on the screen

                System.Console.WriteLine("Download Url = " + downloadUrl);
                // SAVE THIS DOWNLOAD URL INTO THE DB TO ASSOCIATE IT WITH THE IMAGE !!
                Console.WriteLine("***END OF ONSUCCESS SECOND IF ****");
            }

        }

        public void OnFailure(Java.Lang.Exception e)
        {

        }

        public static SearchResults NewInstance()
        {
            var frag = new SearchResults { Arguments = new Bundle() };
            return frag;
        }

    }
}