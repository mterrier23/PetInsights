using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;
using Plugin.CurrentActivity;
using Android.Content;
using System.Collections.Generic;
//using Xamarin.Forms;
using Android.Database;
using Android.Provider;
using CarouselView.FormsPlugin.Droid;
using Xamarin.Forms;
using System.IO;
using Android.Graphics;

namespace PetInsights_all.Droid
{
    [Activity(Label = "PetInsights_all", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;


            FirebaseApp.InitializeApp(this); // TESTING 
            CrossCurrentActivity.Current.Init(this, savedInstanceState); // for currentactivity plugin (media add)
            CarouselViewRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: false);


            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState); //Initialize GoogleMaps here
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #region Image Picker Implementation
        public static int OPENGALLERYCODE = 100;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);


            //If we are calling multiple image selection, enter into here and return photos and their filepaths.
            if (requestCode == OPENGALLERYCODE && resultCode == Result.Ok)
            {
                List<string> images = new List<string>();
                List<Stream> imgStream = new List<Stream>();

                if (data != null)
                {
                    //Separate all photos and get the path from them all individually.
                    ClipData clipData = data.ClipData;
                    if (clipData != null)
                    {
                        for (int i = 0; i < clipData.ItemCount; i++)
                        {
                            ClipData.Item item = clipData.GetItemAt(i);
                            Android.Net.Uri uri = item.Uri;

                            var path = GetRealPathFromURI(uri);


                            // TESTING RESIZE !!
                            byte[] file = this.Resize(uri, path, 60, 60);

                            //byte[] file = File.ReadAllBytes(path);
                            System.IO.Stream strm = new MemoryStream(file);

                            if (path != null)
                            {
                                imgStream.Add(strm);
                                images.Add(path);
                            }
                        }
                    }
                    else
                    {
                        Android.Net.Uri uri = data.Data;
                        var path = GetRealPathFromURI(uri);


                        // TESTING RESIZE !!
                        byte[] file = this.Resize(uri, path, 60, 60);

                        // byte[] file = File.ReadAllBytes(path);
                        System.IO.Stream strm = new MemoryStream(file);


                        if (path != null)
                        {
                            imgStream.Add(strm);
                            images.Add(path);
                        }
                    }

                    //Send our images to the carousel view.
                    MessagingCenter.Send(imgStream, "ImagesStreamAndroid"); // New test
                    MessagingCenter.Send(images, "ImagesSelectedAndroid");  
                }
            }
        }



        // Compresses the image for quicker upload and retrieval
        private byte[] Resize(Android.Net.Uri uri, string path, float maxWidth, float maxHeight)
        {           

            string fileexe = System.IO.Path.GetExtension(path);
            byte[] imageBytes;

            //Get the bitmap.
            var originalImage = BitmapFactory.DecodeFile(path);

            //Set imageSize and imageCompression parameters.
            var imageSize = .86;
            var imageCompression = 15; // NOTE - was 67 - to test compression factors

            //Resize it and then compress it to Jpeg.
            var width = (originalImage.Width * imageSize);
            var height = (originalImage.Height * imageSize);
            var scaledImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, true);


            using (MemoryStream ms = new MemoryStream())
            {
                if (fileexe == ".jpg")
                {
                    scaledImage.Compress(Bitmap.CompressFormat.Jpeg, imageCompression, ms);
                }
                else if (fileexe == ".png")
                    scaledImage.Compress(Bitmap.CompressFormat.Png, imageCompression, ms);
                imageBytes = ms.ToArray();
            }
            return imageBytes;
        }

        /// <summary>
        ///     Get the real path for the current image passed.
        /// </summary>
        public String GetRealPathFromURI(Android.Net.Uri contentURI)
        {
            try
            {
                ICursor imageCursor = null;
                string fullPathToImage = "";

                imageCursor = ContentResolver.Query(contentURI, null, null, null, null);
                imageCursor.MoveToFirst();
                int idx = imageCursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);

                if (idx != -1)
                {
                    fullPathToImage = imageCursor.GetString(idx);
                }
                else
                {
                    ICursor cursor = null;
                    var docID = DocumentsContract.GetDocumentId(contentURI);
                    var id = docID.Split(':')[1];
                    var whereSelect = MediaStore.Images.ImageColumns.Id + "=?";
                    var projections = new string[] { MediaStore.Images.ImageColumns.Data };

                    cursor = ContentResolver.Query(MediaStore.Images.Media.InternalContentUri, projections, whereSelect, new string[] { id }, null);
                    if (cursor.Count == 0)
                    {
                        cursor = ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, projections, whereSelect, new string[] { id }, null);
                    }
                    var colData = cursor.GetColumnIndexOrThrow(MediaStore.Images.ImageColumns.Data);
                    cursor.MoveToFirst();
                    fullPathToImage = cursor.GetString(colData);
                }
                return fullPathToImage;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Xamarin.Forms.Forms.Context, "Unable to get path", ToastLength.Long).Show();
            }
            return null;
        }
        #endregion
    }
}