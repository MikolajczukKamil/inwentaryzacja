using Inwentaryzacja.Models;
using Inwentaryzacja.views.view_chooseRoom;
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
    public partial class WelcomeViewPage : ContentPage
    {
        public WelcomeViewPage()
        {
            InitializeComponent();
        }

        private void scanButtonClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new ChooseRoomPage();
        }
        private async void _AllReportsButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new AllReportsPage());
        }
    }
}