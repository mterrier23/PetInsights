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
    public partial class MainTabbed : TabbedPage
    {
        public MainTabbed()
        {
            Console.WriteLine("arrived to maintabbed");
            InitializeComponent(); // NOTE - was here originally, testing without
           /* NavigationPage navigationPage = new NavigationPage(new MainSearch());
            navigationPage.IconImageSource = "ic_search.png"; // NOTE - might be repeat?
            navigationPage.Title = "Search"; // NOTE - might be repeat?

            Children.Add(navigationPage);
            Children.Add(new MainFaves());
            Children.Add(new MainBreeds());
            Children.Add(new MainProfile()); */
           // all the above is already in the xaml (I THINK !!!)

        }
    }
}