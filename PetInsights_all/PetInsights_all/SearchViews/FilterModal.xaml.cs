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
        List<string> _petTypeFilter; // dog, cat, or exotic
        List<string> _petGenderFilter; // male, female
        List<string> _petAgeFilter; // newborn, young, adult, senior
        List<string> _petSizeFilter; // small, medium, large
        List<string> _petMedicalFilter; // Yes, No
        List<List<string>> _petFilters; // all filters


        public FilterModal(ObservableCollection<Pet> allPets)
        {
            _allPets = allPets;
            _petTypeFilter = new List<string>();
            _petGenderFilter = new List<string>();
            _petAgeFilter = new List<string>();
            _petSizeFilter = new List<string>();
            _petMedicalFilter = new List<string>();
            _petFilters = new List<List<string>>();

            InitializeComponent();


            // initialize the UI
            dogButton.BackgroundColor = Color.White;
            dogButton.BorderColor = Color.LightGray;
            dogButton.TextColor = Color.Black;

            catButton.BackgroundColor = Color.White;
            catButton.BorderColor = Color.LightGray;
            catButton.TextColor = Color.Black;

            exoticButton.BackgroundColor = Color.White;
            exoticButton.BorderColor = Color.LightGray;
            exoticButton.TextColor = Color.Black;


            maleButton.BackgroundColor = Color.White;
            maleButton.BorderColor = Color.LightGray;
            maleButton.TextColor = Color.Black;

            femaleButton.BackgroundColor = Color.White;
            femaleButton.BorderColor = Color.LightGray;
            femaleButton.TextColor = Color.Black;

        }


        // User can select as many as they want!
        private async void PetTypeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = dog, cat, exotic
            if (_petTypeFilter.Contains(buttonName))
            {
                // was already selected, so now we have to unselect it + remove from the list
                _petTypeFilter.Remove(buttonName);
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
            }

            else
            {
                // hasn't been selected yet
                _petTypeFilter.Add(buttonName);
                (sender as Button).BackgroundColor = Color.Blue;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Blue;
            }
        }

        
        private async void PetGenderBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = male, female
            if (_petGenderFilter.Contains(buttonName))
            {
                // was already selected, so now we have to unselect it + remove from the list
                _petGenderFilter.Remove(buttonName);
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
            }

            else
            {
                // hasn't been selected yet
                _petGenderFilter.Add(buttonName);
                (sender as Button).BackgroundColor = Color.Blue;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Blue;
            }
        }

        private async void PetAgeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = male, female
            if (_petAgeFilter.Contains(buttonName))
            {
                // was already selected, so now we have to unselect it + remove from the list
                _petAgeFilter.Remove(buttonName);
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
            }

            else
            {
                // hasn't been selected yet
                _petAgeFilter.Add(buttonName);
                (sender as Button).BackgroundColor = Color.Blue;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Blue;
            }
        }

        private async void PetSizeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = male, female
            if (_petSizeFilter.Contains(buttonName))
            {
                // was already selected, so now we have to unselect it + remove from the list
                _petSizeFilter.Remove(buttonName);
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
            }

            else
            {
                // hasn't been selected yet
                _petSizeFilter.Add(buttonName);
                (sender as Button).BackgroundColor = Color.Blue;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Blue;
            }
        }

        // NOTE -- if user selects yes, add both yes and no to the list -- COMPLETE LATER !!
        private async void PetMedicalBtnClicked(object sender, EventArgs e)
        {
           /* string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = med, nomed (convert to Yes, No

            if (_petAgeFilter.Contains(buttonName) && buttonName.Equals("med"))
            {
                // do something
            }

            if ((sender as Button).BackgroundColor == Color.White)
            {
                (sender as Button).BackgroundColor = Color.Blue;
                _petType = buttonName;
                if (buttonName != medButton.BindingContext as string)
                {
                    medButton.BackgroundColor = Color.White;
                }
                if (buttonName != nomedButton.BindingContext as string)
                {
                    nomedButton.BackgroundColor = Color.White;
                }
            }
            else if ((sender as Button).BackgroundColor == Color.Blue)
            {
                (sender as Button).BackgroundColor = Color.White;
                _petType = null;
            }
           */
        }

        





        //TODO Store filter choices somewhere so it stays when modal is reopened
        /*
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
        */


        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            _petFilters.Add(_petTypeFilter);
            _petFilters.Add(_petGenderFilter);
            _petFilters.Add(_petAgeFilter);
            Console.WriteLine("sending pet filter over! - count = " + _petFilters.Count);
            MessagingCenter.Send(_petFilters, "filtersChanged");
            /*
            if (changeStatus == true)
            {
               MessagingCenter.Send<FilterModal, string>(this, "selectionChanged", genderflag);
            }
            */
            
            await Navigation.PopModalAsync();
        }
        




    }
}
