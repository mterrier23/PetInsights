using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetInsights_all.Services;
using PetInsights_all.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all.SearchViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PetCommentsPage : ContentPage
    {
        public PetCommentsPage(Pet _pet)
        {
            InitializeComponent();
            lstComments.ItemsSource = _pet.Comments;
        }
        async void OnBackButtonClicked(object sender, EventArgs e)
        {
            //await Application.Current.MainPage.Navigation.PopAsync();
            await Navigation.PopAsync();
        }
    }
}