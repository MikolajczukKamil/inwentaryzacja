using Inwentaryzacja.Controllers.Api;
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
        public ScannedItem(List<AllScaning> scannedItems)
        {
            InitializeComponent();
            ReportList.ItemsSource = scannedItems;
        }

        public class AllScaning
        {
            public string ScaningName { get; set; }
            public int ScaningID{ get; set; }
            public int ScaningRoom { get; set; }
            public bool ThisRoom { get; set; }
            public bool Zeskanowano { get; set; }
            public string TextColor
            {
                get
                {
                    if (ThisRoom == true)
                    {
                        return "Green";
                    }
                    return "Red";
                }
            }
            public string Info { 
                get {
                    if (Zeskanowano == true)
                    {
                        if(ThisRoom)
                            return "Zeskanowano";
                        return "Obiekt z innego pomieszczenia";
                    }
                    return "";
                } }
            public string ScaningText { get { return string.Format("{0} {1}", ScaningName, ScaningID); } }
        }

        private void EndScanning(object sender, EventArgs e)
        {
            EnableView(false);
            EnableView(true);
        }

        private async void RetPrevPage(object sender, EventArgs e)
        {
            EnableView(false);
            await Navigation.PopModalAsync();
            EnableView(true);
        }

        private void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string asset = (string)e.SelectedItem;

            DisplayAlert("Przedmiot", asset , "OK");
        }

        private void EnableView(bool state)
        {
            RetButton.IsEnabled = state;
            EndButton.IsEnabled = state;
        }
    }
}