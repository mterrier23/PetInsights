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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleFilterModal : ContentPage
    {
        string genderflag;
        bool changeStatus;
        ObservableCollection<Pet> filtered;
        //ObservableCollection<Pet> _allPets;
        List<string> _petTypeFilter; // dog, cat, or exotic
        List<string> _petGenderFilter; // male, female
        List<string> _petAgeFilter; // newborn, young, adult, senior
        List<string> _petTempermentFilter; // calm, energenic
        List<List<string>> _petFilters; // all filters

        public SimpleFilterModal()
        {


            //_allPets = allPets;
            _petTypeFilter = new List<string>();
            _petGenderFilter = new List<string>();
            _petAgeFilter = new List<string>();
            _petTempermentFilter = new List<string>();
            _petFilters = new List<List<string>>();

            /* initialize the UI */
            InitializeComponent();

            sliderText.Text = "SEARCH RADIUS: 2 miles";

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

            // petGender
            maleButton.BackgroundColor = Color.White;
            maleButton.BorderColor = Color.LightGray;
            maleButton.TextColor = Color.Black;

            femaleButton.BackgroundColor = Color.White;
            femaleButton.BorderColor = Color.LightGray;
            femaleButton.TextColor = Color.Black;

            // petAge
            newbornButton.BackgroundColor = Color.White;
            newbornButton.BorderColor = Color.LightGray;
            newbornButton.TextColor = Color.Black;

            youngButton.BackgroundColor = Color.White;
            youngButton.BorderColor = Color.LightGray;
            youngButton.TextColor = Color.Black;

            adultButton.BackgroundColor = Color.White;
            adultButton.BorderColor = Color.LightGray;
            adultButton.TextColor = Color.Black;

            seniorButton.BackgroundColor = Color.White;
            seniorButton.BorderColor = Color.LightGray;
            seniorButton.TextColor = Color.Black;


            // petTemperment
            calmButton.BackgroundColor = Color.White;
            calmButton.BorderColor = Color.LightGray;
            calmButton.TextColor = Color.Black;

            energenicButton.BackgroundColor = Color.White;
            energenicButton.BorderColor = Color.LightGray;
            energenicButton.TextColor = Color.Black;

            

        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            sliderText.Text = "SEARCH RADIUS: " +  (int)args.NewValue + " miles";
            
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

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            _petFilters.Add(_petTypeFilter);
            _petFilters.Add(_petGenderFilter);
            _petFilters.Add(_petAgeFilter);
            _petFilters.Add(_petTempermentFilter);
            Console.WriteLine("sending pet filter over! - count = " + _petFilters.Count);

            // inserting page before caused some issues...
            //await Application.Current.MainPage.Navigation.PopModalAsync(); // testing something new
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }

}