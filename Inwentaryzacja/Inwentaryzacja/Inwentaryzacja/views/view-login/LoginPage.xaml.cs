using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.controllers.session;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        APIController api;
        public LoginPage()
        {
            api = new APIController();
            api.ErrorEventHandler += LoginFail;
            InitializeComponent();
        }

        private async void _loginButton_Clicked(object sender, EventArgs e)
        {
            if(await api.LoginUser(_login.Text, _password.Text))
            {
                var session = new SessionController(api);
                session.SaveSessionToken();

                if(Navigation.NavigationStack.Count == 0)
                {
                    App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
                }
                else
                {
                    await Navigation.PopAsync();
                }  
            }
        }

        private async void LoginFail(object sender, ErrorEventArgs e)
        {
            await DisplayAlert("Błąd logowania", e.MessageForUser, "OK");
        }
    }
}
