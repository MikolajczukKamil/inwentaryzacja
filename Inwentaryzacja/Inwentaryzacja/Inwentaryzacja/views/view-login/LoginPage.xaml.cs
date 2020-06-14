using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.controllers.session;
using System.Threading.Tasks;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// Klasa odpowiadajaca za widok okna logowania
    /// </summary>
    public partial class LoginPage : ContentPage
    {
        APIController api;
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        public LoginPage()
        {
            api = new APIController();
            BindingContext = this;
            api.ErrorEventHandler += LoginFail;
            InitializeComponent();
        }

        /// <summary>
        /// Funkcja odpowiadajaca za wyswietlenie wstepnego widoku okna
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            OnstartPage();
        }
        /// <summary>
        /// Funkcja odpowiadajaca za utworzenie sesji logowania uzytkownika
        /// </summary>
        private async void OnstartPage()
        {

            await Task.Run(() =>
            {
                if (Navigation.NavigationStack.Count == 0)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var session = new SessionController(api);
                        if (session.IsLogin() && (await api.getAssetInfo(1)) != null)
                        {
                            NextPage();
                        }
                        else
                        {
                            PageIsBusy(false);
                        }
                    });
                }
                else
                {
                    PageIsBusy(false);
                }
            });  
        }
        /// <summary>
        /// Funkcja odpowiadajaca za obsluge okna logowania 
        /// </summary>
        /// <param name="state">stan okna</param>
        private void PageIsBusy(bool state)
        {
            _login.IsEnabled = !state;
            LoadingScreen.IsVisible = state;
        }
        /// <summary>
        /// Funkcja odpowiadajaca za przejscie do nastepnego okna
        /// </summary>
        private void NextPage()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Navigation.NavigationStack.Count == 0)
                {
                    App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
                }
                else
                {
                    await Navigation.PopAsync();
                }
            });
        }
        /// <summary>
        /// Funkcja odpowiadajaca za obsluge przycisku logowania
        /// </summary>
        private void _loginButton_Clicked(object sender, EventArgs e)
        {
            PageIsBusy(true);

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await api.LoginUser(_login.Text, _password.Text))
                {
                    var session = new SessionController(api);
                    session.SaveSessionToken();

                    NextPage();
                }
            });
        }
        /// <summary>
        /// Funkcja odpowiadajaca za wyswietlenie komunikatu w przypadku nieudanej proby logowania
        /// </summary>
        private async void LoginFail(object sender, ErrorEventArgs e)
        {
            await DisplayAlert("Błąd logowania", e.MessageForUser, "OK");

            PageIsBusy(false);
        }
    }
}
