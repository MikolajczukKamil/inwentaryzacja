using Inwentaryzacja.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllReportsPage : ContentPage
    {

        APIController api;
        public AllReportsPage()
        {
            api = new APIController();
            InitializeComponent();
        }
        private async void back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
        }
    }
}