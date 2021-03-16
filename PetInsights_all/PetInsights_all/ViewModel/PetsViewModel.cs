using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using PetInsights_all.Model;
using PetInsights_all.Services;

namespace PetInsights_all.ViewModel
{
    class PetsViewModel : BaseViewModel
    {
        public string ImgIcon { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        // testing if this allows changes !!

        private ObservableCollection<string> _media = new ObservableCollection<string>();

        public ObservableCollection<string> Media
        {
            get { return _media; }
            set
            {
                _media = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Pet> _comments = new ObservableCollection<Pet>();

        public ObservableCollection<Pet> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                OnPropertyChanged();
            }
        }



        private DBFirebase services;

        private ObservableCollection<Pet> _pets = new ObservableCollection<Pet>();

        public ObservableCollection<Pet> Pets
        {
            get { return _pets; }
            set 
            {
                _pets = value;
                OnPropertyChanged();
            }
        }

        public PetsViewModel()
        {
            services = new DBFirebase();
            Pets = services.getPets(); 
        }
    }
}
