using Inwentaryzacja.models;
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
        public ScannedItem(List<Asset> scannedItem)
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
            Asset asset = (Asset)e.SelectedItem;

            DisplayAlert("Przedmiot", asset.name , "OK");
        }
    }
}