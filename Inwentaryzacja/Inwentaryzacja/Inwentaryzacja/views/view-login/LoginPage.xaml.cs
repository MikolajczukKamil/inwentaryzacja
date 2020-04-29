using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Inwentaryzacja;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using Inwentaryzacja.controllers;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void _loginButton_Clicked(object sender, EventArgs e)
        {
            _login.Text = "test";
            _password.Text = "password";

            var apiController = new APIController();
            apiController.ErrorEventHandler += LoginFail;

            if(await apiController.loginUser(_login.Text, _password.Text))
            {
                App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
            }
        }

        private async void LoginFail(object sender, ErrorEventArgs e)
        {
            await DisplayAlert("Błąd logowania", e.messageForUser, "OK");
        }
    }
}