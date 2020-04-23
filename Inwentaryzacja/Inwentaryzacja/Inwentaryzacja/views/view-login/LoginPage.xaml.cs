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
                //var userPw = string.Format("{0}:{1}", _login.Text, _password.Text);
                //var header = Convert.ToBase64String(Encoding.UTF8.GetBytes(userPw));
                var header = "kjdhsfuyhreufh";

                HttpResponseMessage response = null;
                
                //App.clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", header);
                App.clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

                try
                {
                    response = await App.clientHttp.GetAsync("https://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/index.php");
                }
                catch (Exception failConnection)
                {
                    await DisplayAlert("Nie znaleziono serwera", "Sprawdź połączenie z internetem lub zmień połączenie sieciowe", "OK");
                }

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    //var lista = JsonConvert.DeserializeObject<List<Rekord>>(content);
                    await DisplayAlert("Dane", content, "OK");
                    ///await DisplayAlert("Dane", response.Headers.GetValues("X-Request-ID").First().ToString(), "OK");
                }
                else
                {
                    await DisplayAlert("Błędny login lub hasło", "Spróbuj ponownie wprowadzić login i hasło", "OK");
                }
            }
            else
            {
                await DisplayAlert("Brak Internetu", "Sprawdź połączenie z internetem", "OK");
            }
        }
    }
}