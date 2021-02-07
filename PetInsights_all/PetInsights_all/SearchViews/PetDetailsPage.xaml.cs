using PetInsights_all.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetInsights_all.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PetDetailsPage : ContentPage
    {
        DBFirebase services;
        public PetDetailsPage(Pet pet)
        {
            InitializeComponent();
            BindingContext = pet;
            services = new DBFirebase();
        }

        public async void BtnUpdate_Pet(object sender, EventArgs e)
        {
            Console.WriteLine("******pressed button but before update pet call ****");
            await services.UpdatePet(
                Name.Text, int.Parse(Age.Text));
            Console.WriteLine("After update pet function call");
            await Navigation.PopAsync();
        }
    }
}