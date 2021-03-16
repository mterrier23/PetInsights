using PetInsights_all.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PetInsights_all.ViewModel;
using PetInsights_all.Search;
using System.Collections.ObjectModel;
using PetInsights_all.SearchViews;


namespace PetInsights_all.SearchViews
{
    public partial class FilterModal : ContentPage
    {
        string genderflag;
        bool changeStatus;
        ObservableCollection<Pet> filtered; 
        ObservableCollection<Pet> _allPets;


        public FilterModal(ObservableCollection<Pet> allPets)
        {
            _allPets = allPets;
            InitializeComponent();
        }

        
        //TODO Store filter choices somewhere so it stays when modal is reopened
        void OnSelectionChange(object sender, CheckedChangedEventArgs args)
        {
            if (Male.IsChecked && !Female.IsChecked)
            {
                genderflag = "Male";
            }

            else if (Female.IsChecked && !Male.IsChecked)
            {
                genderflag = "Female";
            }

            else
            {
                genderflag = "Both";
            }

            changeStatus = true;

        }


        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            if (changeStatus == true)
            {
                MessagingCenter.Send<FilterModal, string>(this, "selectionChanged", genderflag);
            }
            
            await Navigation.PopModalAsync();
        }




    }
}
