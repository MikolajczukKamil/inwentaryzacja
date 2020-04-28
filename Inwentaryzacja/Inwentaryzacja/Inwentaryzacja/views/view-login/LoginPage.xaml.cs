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
            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                _login.Text = "test";
                _password.Text = "password";

                try
                {
                    var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/login/login.php");
                    var data = "{\"login\":\"" + _login.Text + "\", \"password\":\"" + _password.Text + "\"}";
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await App.clientHttp.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var header = response.Headers.GetValues("Authorization").First().ToString();
                        header = header.Remove(0,7);
                        App.clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);
                        await Navigation.PushAsync(new WelcomeViewPage());
                    }
                    else
                    {
                        await DisplayAlert("Błędny login lub hasło", "Spróbuj ponownie wprowadzić login i hasło", "OK");
                    }
                }
                catch (Exception failConnection)
                {
                    await DisplayAlert("Nie znaleziono serwera", "Sprawdź połączenie z internetem lub zmień połączenie sieciowe", "OK");
                }   
            }
            else
            {
                await DisplayAlert("Brak Internetu", "Sprawdź połączenie z internetem", "OK");
            }
        }
    }
}