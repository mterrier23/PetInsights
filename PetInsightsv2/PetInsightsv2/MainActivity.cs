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
using Android.Content;

namespace PetInsightsv2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        Button addContentButton;

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

            var gridview = FindViewById<GridView>(Resource.Id.gridView);
            gridview.Adapter = new ImageAdapter(this);


            addContentButton = (Button)FindViewById(Resource.Id.addContent);
            addContentButton.Click += addContentButton_Click;
        }


        private void addContentButton_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(AddContentActivity));
            StartActivity(intent);

        }
    }
}