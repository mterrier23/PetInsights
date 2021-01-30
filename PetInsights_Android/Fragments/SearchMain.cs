using Android;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using System;
using Android.Content;
using Android.Widget;
using System;

namespace PetInsightsv2.Fragments
{
    public class SearchMain : Fragment
    {

        Button searchPetsButton;
        Button petsMapButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
            // main page? TO DO !!
        }

        public static SearchMain NewInstance()
        {
            var frag1 = new SearchMain { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.search_main, container, false);

            searchPetsButton = view.FindViewById<Button>(Resource.Id.search_button);
            searchPetsButton.Click += delegate
            {
                //Intent toNextPage = new Intent(Context, typeof(AddContentActivity)); /*added this for testing */
                //Activity.StartActivity(toNextPage);
                // *** NOTE *** ^ The above activity uploads just fine, no auth issues, but the below activity has auth issues. and we need to do below actvity
                ((MainActivity)Activity).LoadFragment(Resource.Id.search_button);
            };

            petsMapButton = view.FindViewById<Button>(Resource.Id.map_button);
            petsMapButton.Click += delegate
            {
                //Intent toNextPage = new Intent(Context, typeof(AddContentActivity)); /*added this for testing */
                //Activity.StartActivity(toNextPage);
                // *** NOTE *** ^ The above activity uploads just fine, no auth issues, but the below activity has auth issues. and we need to do below actvity
                ((MainActivity)Activity).LoadFragment(Resource.Id.map_button);
            };

            return view;
        }


    }
}


