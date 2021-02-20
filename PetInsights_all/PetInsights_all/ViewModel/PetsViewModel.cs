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

        private DBFirebase services;

        public Command AddPetCommand { get; }

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
            Pets = services.getPets(); // NOTE - change here
            if (Pets != null)
                Console.WriteLine("pets isn't null");
            else Console.WriteLine("pets is null");
            // NOTE - pets isn't null!
            AddPetCommand = new Command(async () => await addPetAsync(ImgIcon, Name, Age));
        }

        // used to upload pets to database (NOTE - eventually change to upload comment and image!
        public async Task addPetAsync(string ImgIcon, string Name, int Age)
        {
            await services.AddPetTask(Name, Age, ImgIcon);
        }
    }
}
