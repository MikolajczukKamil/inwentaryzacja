using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.controllers.session;
using System.Threading.Tasks;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        APIController api;
        public LoginPage()
        {
            api = new APIController();
            BindingContext = this;
            api.ErrorEventHandler += LoginFail;
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if(Navigation.NavigationStack.Count==0)
            {
                Task.Run(() =>
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
                            LoadingScreen.IsVisible = false;
                        }
                    });
                });
            }
            else
            {
                LoadingScreen.IsVisible = false;
            }
        }

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

                LoadingScreen.IsVisible = false;
                _login.IsEnabled = true;
            });
        }

        private async void _loginButton_Clicked(object sender, EventArgs e)
        {
            _login.IsEnabled = false;
            LoadingScreen.IsVisible = true;

            if (await api.LoginUser(_login.Text, _password.Text))
            {
                var session = new SessionController(api);
                session.SaveSessionToken();

                NextPage();
            }
        }

        private async void LoginFail(object sender, ErrorEventArgs e)
        {
            await DisplayAlert("Błąd logowania", e.MessageForUser, "OK");

            LoadingScreen.IsVisible = false;
            _login.IsEnabled = true;
        }
    }
}
