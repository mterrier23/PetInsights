using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetInsights_all.SearchViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrgListView : ContentPage
    {
        public OrgListView()
        {
            InitializeComponent();
        }

        async void OnFilterButtonClicked(object sender, EventArgs e)
        {
            // let them change location if needed
        }
    }
}