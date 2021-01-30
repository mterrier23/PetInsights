using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetLocation : ContentPage
    {
        public GetLocation()
        {
            InitializeComponent();
        }
        private async void FindPetsButton_OnClicked(object sender, EventArgs e)
        {
            Console.WriteLine("pressing findbets button");

            await Navigation.PushModalAsync(new MainTabbed());
            //await Navigation.PushAsync(new MainTabbed()); // NOTE - change this bc don't need navigation for this page
        }
    }
}