using Android;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace PetInsightsv2.Fragments
{
    public class BreedMain : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static BreedMain NewInstance()
        {
            var frag2 = new BreedMain { Arguments = new Bundle() };
            return frag2;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.breed_main, null);
        }
    }
}