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
using Android.Gms.Maps;

using com.google.android.gms.maps.CameraUpdateFactory;
using com.google.android.gms.maps.GoogleMap;
using com.google.android.gms.maps.MapFragment;
using com.google.android.gms.maps.MapView;
using google.android.gms.maps.OnMapReadyCallback;
using com.google.android.gms.maps.SupportMapFragment;
using com.google.android.gms.maps.model.LatLng;
using com.google.android.gms.maps.model.MarkerOptions;

namespace PetInsightsv2.Fragments
{
    public class PetMap : Fragment, 
    {

        MapView mapView;
        private GoogleMap googleMap;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.petsmap, container, false);
            mapView = view.FindViewById<MapView>(Resource.Id.map);
            mapView.OnCreate(savedInstanceState);

            mapView.OnResume();// needed to get the map to display immediately

            //googleMap = mapView.getMap(); (java version)
            googleMap = mapView.GetMapAsync(this);
            // latitude and longitude
            double latitude = 17.385044;
            double longitude = 78.486671;

            // create marker
            MarkerOptions marker = new MarkerOptions().position(
                    new LatLng(latitude, longitude)).title("Hello Maps");

            // Changing marker icon
            marker.icon(BitmapDescriptorFactory
                    .defaultMarker(BitmapDescriptorFactory.HUE_ROSE));

            // adding marker
            googleMap.addMarker(marker);
            CameraPosition cameraPosition = new CameraPosition.Builder()
                    .target(new LatLng(17.385044, 78.486671)).zoom(12).build();
            googleMap.animateCamera(CameraUpdateFactory
                    .newCameraPosition(cameraPosition));

            // Perform any camera updates here

             return base.OnCreateView(inflater, container, savedInstanceState);



        }

        public static PetMap NewInstance()
        {
            var frag1 = new PetMap { Arguments = new Bundle() };
            return frag1;
        }

    }
}