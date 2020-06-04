using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
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

        private async void scanButtonClicked(object sender, EventArgs e)
        {
            EnableView(false);
            await Navigation.PushAsync(new ChooseRoomPage());
            EnableView(true);
        }
        
        private async void _AllReportsButton_Clicked(object sender, EventArgs e)
        {
            EnableView(false);
            await Navigation.PushAsync(new AllReportsPage());
            EnableView(true);
        }

        private async void LogoutButtonClicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("Wylogowywanie", "Czy na pewno chcesz się wylogować?", "Tak", "Nie"))
            {
                var session = new SessionController(new APIController());
                session.RemoveSession();
                App.Current.MainPage = new LoginPage();
            }
        }

        private void EnableView(bool state)
        {
            ScanButton.IsEnabled = state;
            AllReportsButton.IsEnabled = state;
            LogoutButton.IsEnabled = state;
        }
    }
}