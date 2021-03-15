using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace PetInsights_all.SearchViews
{
    public partial class FilterModal : ContentPage
    {
        public FilterModal()
        {
            InitializeComponent();
        }


        void OnSelectionChange(object sender, CheckedChangedEventArgs args)
        {
            var chkbox = (CheckBox)sender;
            Console.WriteLine(chkbox.ClassId);

        }


        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            Console.WriteLine(args);
            await Navigation.PopModalAsync();
        }
    }
}
