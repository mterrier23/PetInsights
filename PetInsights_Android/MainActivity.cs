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
using Android.Support.Design.Widget;
using PetInsightsv2.Fragments;

namespace PetInsightsv2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        StorageReference storageReference;
        private string downloadUrl;
        BottomNavigationView bottomNavigation;

        Android.Support.V4.App.Fragment fragmentSearch;
        Android.Support.V4.App.Fragment fragmentFave;
        Android.Support.V4.App.Fragment fragmentBreed;
        Android.Support.V4.App.Fragment fragmentProfile;
        Android.Support.V4.App.Fragment searchResultsFragment;
        Android.Support.V4.App.Fragment PetMapFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main); /* testing TESTING */

            /* Added for bottom nav bar */
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);
            }

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_main);
            fragmentSearch = SearchMain.NewInstance();
            fragmentFave = FaveMain.NewInstance();
            fragmentBreed = BreedMain.NewInstance();
            fragmentProfile = ProfileMain.NewInstance();
            searchResultsFragment = SearchResults.NewInstance();
            PetMapFragment = PetMap.NewInstance();

            /* TESTING */
            /* bottomNavigation.NavigationItemSelected += (s, e) =>
             {

                 Android.Support.V4.App.Fragment activeFragment = null;

                 switch (e.Item.ItemId)
                 {
                     case Resource.Id.menu_home:
                         activeFragment = fragmentSearch;
                         break;
                     case Resource.Id.menu_fave:
                         activeFragment = fragmentFave;
                         break;
                     case Resource.Id.menu_breed:
                         activeFragment = fragmentBreed;
                         break;
                     case Resource.Id.menu_profile:
                         activeFragment = fragmentProfile;
                         break;
                 }

                 if (activeFragment == null)
                     return;

                 //SupportFragmentManager
                         .BeginTransaction()
                         .Hide(activeFragment)
                         .Show(fragmentSearch)
                         .Commit();

                 SupportFragmentManager.BeginTransaction()
                   .Replace(Resource.Id.content_frame, activeFragment)
                   .Commit();

             }; 
            */


            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            // Load the first fragment on creation
            LoadFragment(Resource.Id.menu_home);
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }
       



        /* COMMENTED OUT FOR TESTING */
        /*var gridview = FindViewById<GridView>(Resource.Id.gridView);
         gridview.Adapter = new ImageAdapter(this);


         addContentButton = (Button)FindViewById(Resource.Id.addContent);
         addContentButton.Click += addContentButton_Click;
        */


        public void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment activeFragment = null;
            switch (id)
            {
                case Resource.Id.menu_home:
                    activeFragment = fragmentSearch;
                    break;
                case Resource.Id.menu_fave:
                    activeFragment = fragmentFave;
                    break;
                case Resource.Id.menu_breed:
                    activeFragment = fragmentBreed;
                    break;
                case Resource.Id.menu_profile:
                    activeFragment = fragmentProfile;
                    break;
                case Resource.Id.search_button:
                    activeFragment = searchResultsFragment;
                    break;
                case Resource.Id.map_button:
                    activeFragment = PetMapFragment;
                    break;
            }

            if (activeFragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
                /* below is testing for backstack handler... but doesn't work yet? */
                //.Add(activeFragment, "activeFragment")
                // add this transaction to the back stack
                //.AddToBackStack(null)
                // not sure if we still need this .replace...
                .Replace(Resource.Id.content_frame, activeFragment)
                .Commit();

        
            // tutorial says to use this instead, but it's not working correctly :/
            //SupportFragmentManager
            //            .BeginTransaction()
            //            .Hide(activeFragment)
            //            .Show(fragmentSearch)
            //            .Commit();
            
        }
    


        private void addContentButton_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(AddContentActivity));
            StartActivity(intent);

        }
 
    }
}