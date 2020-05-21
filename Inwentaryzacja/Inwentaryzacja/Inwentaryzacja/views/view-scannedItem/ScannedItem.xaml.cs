using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_scannedItem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannedItem : ContentPage
    {
        public ScannedItem(List<string> scannedItem)
        {
            InitializeComponent();

            _listView.ItemsSource = scannedItem;
        }

        private void EndScanning(object sender, EventArgs e)
        {

        }

        private async void RetPrevPage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string asset = (string)e.SelectedItem;

            DisplayAlert("Przedmiot", asset , "OK");
        }

        private async void _logOutButton_Clicked(object sender, EventArgs e)
        {
            string TokenFileName = "Token.txt";    //Nazwa pliku gdzie zapisuje token                             
            string WhereTokenIs = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Sciezka gdzie jest plik Token.txt	
            string DeleteTokenFromPath = Path.Combine(WhereTokenIs, TokenFileName); // Sciezka + Nazwa pliku


            if (!File.Exists(DeleteTokenFromPath))
            {
                try
                {
                    // Jesli istnieje usuwam
                    File.Delete(DeleteTokenFromPath); // Usuwam plik Token.txt          
                    if (Navigation.NavigationStack.Count == 0)
                    {                                 // Redirect strona logowania
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    else
                    {
                        await Navigation.PopAsync();
                    }
                }                                   // Jesli plik nie istnieje komunikat 
                catch { await DisplayAlert("Uzytkownik nie jest zalagowany", "Zalogowac sie?", "ok"); }
            }
        }
    }
}