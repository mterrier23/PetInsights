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
        //ObservableCollection<Pet> _allPets;
        List<string> _petTypeFilter; // dog, cat, or exotic
        List<string> _petGenderFilter; // male, female
        List<string> _petAgeFilter; // newborn, young, adult, senior
        List<string> _petSizeFilter; // small, medium, large
        List<string> _petTempermentFilter; // calm, energenic
        List<string> _petMedicalFilter; // Yes, No
        List<string>[] _petFilters_; 
        /* 0 - Distance
         * 1 - Age Range (2 years - 8 years, 8 years +
         * 2 - Apartment Friendly (Yes, No)
         * 3 - Breed (string)
         * 4 - Hypoallergenic (Yes, No)
         * 5 - Maintenance (Low, Moderate, High)
         * 6 - Medical Condition (Yes, No)
         * 7 - Type (cat, dog, exotic)
         * 8 - Potty trained (Yes, No)
         * 9 - sex (Male, Female)
         * 10 - Sheds (Yes, No)
         * 11 - Size (small, medium, large)
         */
        
        // removing for testing the above !!!
        //List<List<string>> _petFilters; // all filters


        /* public FilterModal(ObservableCollection<Pet> allPets) */
        public FilterModal()
        {
            //_allPets = allPets;
            _petTypeFilter = new List<string>();
            _petGenderFilter = new List<string>();
            _petAgeFilter = new List<string>();
            _petSizeFilter = new List<string>();
            _petTempermentFilter = new List<string>();
            _petMedicalFilter = new List<string>();
            _petFilters_ = new List<string>[11];

            //_petFilters = new List<List<string>>();


            InitializeComponent();

            /* initialize the UI */

            // petType
            dogButton.BackgroundColor = Color.White;
            dogButton.BorderColor = Color.LightGray;
            dogButton.TextColor = Color.Black;
            
            catButton.BackgroundColor = Color.White;
            catButton.BorderColor = Color.LightGray;
            catButton.TextColor = Color.Black;

            exoticButton.BackgroundColor = Color.White;
            exoticButton.BorderColor = Color.LightGray;
            exoticButton.TextColor = Color.Black;


            // HARDCODE OOPS
            catButton.BackgroundColor = Color.Orange;
            catButton.BorderColor = Color.Orange;
            catButton.TextColor = Color.White;

            exoticButton.BackgroundColor = Color.Orange;
            exoticButton.BorderColor = Color.Orange;
            exoticButton.TextColor = Color.White;


            // petGender
            maleButton.BackgroundColor = Color.White;
            maleButton.BorderColor = Color.LightGray;
            maleButton.TextColor = Color.Black;

            /*
            femaleButton.BackgroundColor = Color.White;
            femaleButton.BorderColor = Color.LightGray;
            femaleButton.TextColor = Color.Black;
            */

            femaleButton.BackgroundColor = Color.Orange;
            femaleButton.BorderColor = Color.Orange;
            femaleButton.TextColor = Color.White;


            // petAge
            newbornButton.BackgroundColor = Color.White;
            newbornButton.BorderColor = Color.LightGray;
            newbornButton.TextColor = Color.Black;

            /*
            youngButton.BackgroundColor = Color.White;
            youngButton.BorderColor = Color.LightGray;
            youngButton.TextColor = Color.Black;
            
            adultButton.BackgroundColor = Color.White;
            adultButton.BorderColor = Color.LightGray;
            adultButton.TextColor = Color.Black;
            */

            youngButton.BackgroundColor = Color.Orange;
            youngButton.BorderColor = Color.Orange;
            youngButton.TextColor = Color.White;

            adultButton.BackgroundColor = Color.Orange;
            adultButton.BorderColor = Color.Orange;
            adultButton.TextColor = Color.White;

            seniorButton.BackgroundColor = Color.White;
            seniorButton.BorderColor = Color.LightGray;
            seniorButton.TextColor = Color.Black;


            // petTemperment
            /*
            calmButton.BackgroundColor = Color.White;
            calmButton.BorderColor = Color.LightGray;
            calmButton.TextColor = Color.Black;
            */

            energenicButton.BackgroundColor = Color.White;
            energenicButton.BorderColor = Color.LightGray;
            energenicButton.TextColor = Color.Black;


            calmButton.BackgroundColor = Color.Orange;
            calmButton.BorderColor = Color.Orange;
            calmButton.TextColor = Color.White;


            smallButton.BackgroundColor = Color.White;
            smallButton.BorderColor = Color.LightGray;
            smallButton.TextColor = Color.Black;

            mediumButton.BackgroundColor = Color.White;
            mediumButton.BorderColor = Color.LightGray;
            mediumButton.TextColor = Color.Black;

            largeButton.BackgroundColor = Color.White;
            largeButton.BorderColor = Color.LightGray;
            largeButton.TextColor = Color.Black;

            medButton.BackgroundColor = Color.White;
            medButton.BorderColor = Color.LightGray;
            medButton.TextColor = Color.Black;

            nomedButton.BackgroundColor = Color.White;
            nomedButton.BorderColor = Color.LightGray;
            nomedButton.TextColor = Color.Black;


            sliderText.Text = "SEARCH RADIUS: 2 miles";

        }


        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            sliderText.Text = "SEARCH RADIUS: " + (int)args.NewValue + " miles";

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
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
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
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
            }
        }

        private async void PetAgeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = newborn, young, adult, senior
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
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
            }
        }

        private async void PetSizeBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = small, medium, large
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
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
            }
        }


        private async void PetTempermentBtnClicked(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).BindingContext as string; // potential binding contexts = calm, energenic
            if (_petTempermentFilter.Contains(buttonName))
            {
                // was already selected, so now we have to unselect it + remove from the list
                _petTempermentFilter.Remove(buttonName);
                (sender as Button).BackgroundColor = Color.White;
                (sender as Button).TextColor = Color.Black;
                (sender as Button).BorderColor = Color.LightGray;
            }

            else
            {
                // hasn't been selected yet
                _petTempermentFilter.Add(buttonName);
                (sender as Button).BackgroundColor = Color.Orange;
                (sender as Button).TextColor = Color.White;
                (sender as Button).BorderColor = Color.Orange;
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
                (sender as Button).BackgroundColor = Color.Orange;
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
            else if ((sender as Button).BackgroundColor == Color.Orange)
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
            _petFilters_[7]=(_petTypeFilter);
            _petFilters_[9] = (_petGenderFilter);
            _petFilters_[1] = (_petAgeFilter);



            MessagingCenter.Send(_petFilters_, "filtersChanged");

            //_petFilters.Add(_petGenderFilter);
            //_petFilters.Add(_petAgeFilter);
            //Console.WriteLine("sending pet filter over! - count = " + _petFilters.Count);


            //MessagingCenter.Send(_petFilters, "filtersChanged");
            /*
            if (changeStatus == true)
            {
               MessagingCenter.Send<FilterModal, string>(this, "selectionChanged", genderflag);
            }
            */

            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
