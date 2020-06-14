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
    /// <summary>
    /// Klasa odpowiadajaca za widok okna powitalnego aplikacji
    /// </summary>
    public partial class WelcomeViewPage : ContentPage
    {
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        public WelcomeViewPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Funkcja odpowiadajaca za obsluge przycisku skanowania
        /// </summary>
        private async void scanButtonClicked(object sender, EventArgs e)
        {
            EnableView(false);
            await Navigation.PushAsync(new ChooseRoomPage());
            EnableView(true);
        }
        /// <summary>
        /// Funkcja odpowiadajaca za obsluge przycisku wyswietlenia wszystkich raportow
        /// </summary>
        private async void _AllReportsButton_Clicked(object sender, EventArgs e)
        {
            EnableView(false);
            await Navigation.PushAsync(new AllReportsPage());
            EnableView(true);
        }
        /// <summary>
        /// Funkcja odpowiadajaca za obsluge przycisku wylogowania
        /// </summary>
        private async void LogoutButtonClicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("Wylogowywanie", "Czy na pewno chcesz się wylogować?", "Tak", "Nie"))
            {
                var session = new SessionController(new APIController());
                session.RemoveSession();
                App.Current.MainPage = new LoginPage();
            }
        }
        /// <summary>
        /// Funkcja odpowiadajaca za umozliwienie wyswietlenia widoku okna
        /// </summary>
        private void EnableView(bool state)
        {
            ScanButton.IsEnabled = state;
            AllReportsButton.IsEnabled = state;
            LogoutButton.IsEnabled = state;
        }
    }
}