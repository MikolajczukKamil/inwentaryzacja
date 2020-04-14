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
        List<Item> scannedItem;

        public ScannedItem(List<Item> scannedItem)
        {
            InitializeComponent();

            this.scannedItem = scannedItem;
            _listView.ItemsSource = this.scannedItem;
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
            Item item = (Item)e.SelectedItem;

            DisplayAlert("Przedmiot", item.Text , "OK");
        }
    }
}