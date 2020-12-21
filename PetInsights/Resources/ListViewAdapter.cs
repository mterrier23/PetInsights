using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PetInsights.Resources.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetInsights.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtName { get; set; }
        public TextView txtAge { get; set; }
        public TextView txtSex { get; set; }

    }
    public class ListViewAdapter:BaseAdapter
    {
        private Activity activity;
        private List<Animal> lstAnimal;
        public ListViewAdapter(Activity activity, List<Animal> lstAnimal)
        {
            this.activity = activity;
            this.lstAnimal = lstAnimal;
        }

        public override int Count
        {
            get
            {
                return lstAnimal.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return lstAnimal[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_item, parent, false);
            var txtName = view.FindViewById<TextView>(Resource.Id.textView1);
            var txtAge = view.FindViewById<TextView>(Resource.Id.textView2);
            var txtSex = view.FindViewById<TextView>(Resource.Id.textView3);

            txtName.Text = lstAnimal[position].Name;
            txtAge.Text = "" + lstAnimal[position].Age;
            txtSex.Text = lstAnimal[position].Sex;


            return view;
        }
    }
}