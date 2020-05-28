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
            await Navigation.PushAsync(new ChooseRoomPage());
        }
        private async void _AllReportsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllReportsPage());
        }

        private async void LogoutClicked(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Wylogowywanie", "Czy na pewno chcesz się wylogować?", "Tak", "Nie");
            if(response)
            {
                var session = new SessionController(new APIController());
                session.RemoveSession();
                App.Current.MainPage = new LoginPage();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            LogoutClicked(this, null);

            return true;
        }
    }
}