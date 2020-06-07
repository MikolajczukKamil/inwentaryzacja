using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_scannedItem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannedItem : ContentPage
    {
        APIController api;
        RoomEntity ScanningRoom;
        List<AllScaning> allScaning;
        public ScannedItem(List<AllScaning> scannedItems, RoomEntity scanningRoom)
        {
            InitializeComponent();
            allScaning = scannedItems;
            api = new APIController();
            ScanningRoom = scanningRoom;
            BindingContext = this;
            ScannedInRoomTopic.Text = "Zeskanowane z sali " + ScanningRoom.name;
            UnscannedInRoomTopic.Text = "Niezeskanowane z sali " + ScanningRoom.name;
            ShowInfo();
        }

        public class AllScaning
        {
            public ReportPositionPrototype reportPositionPrototype;

            public AssetEntity AssetEntity;
            private RoomEntity ScanningRoom;
            public bool Approved = false;
            public int? AssetRoom = null;

            public AllScaning(AssetEntity assetEntity, RoomEntity assetRoom, RoomEntity scanningRoom)
            {
                if (assetRoom != null)
                {
                    AssetRoom = assetRoom.id;
                }
                reportPositionPrototype = new ReportPositionPrototype(assetEntity, assetRoom, false);
                AssetEntity = assetEntity;
                ScannedId = assetEntity.id;
                ScanningRoom = scanningRoom;
            }
            public void ItemMoved()
            {
                Approved = true;
                reportPositionPrototype.present = true;
                AssetRoom = ScanningRoom.id;
            }

            public string ScaningText { get { return string.Format("{0} {1}", AssetEntity.type.name, AssetEntity.id); } }
            public int ScannedId { get; set; }
            public string RoomId { get { if (AssetRoom != null) return AssetRoom.ToString(); return "brak"; } }

        }

        private void ShowInfo()
        {
            int[] items = { 0, 0, 0, 0, 0, 0 };//c k m p s t
            string[] types = { "Komputer:", "Krzesło:", "Monitor:", "Projektor:", "Stół:", "Tablica:" };
            foreach (AllScaning item in allScaning)
            {
                if (item.reportPositionPrototype.present)
                {
                    items = CheckAmount(items, item.AssetEntity.type.id);
                }
            }
            ScannedInRoomLabel.Text = "";
            ScannedInRoomAmount.Text = "";
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] > 0)
                {
                    ScannedInRoomLabel.Text += types[i] + "\n";
                    ScannedInRoomAmount.Text += items[i];
                    if (items[i] == 1)
                        ScannedInRoomAmount.Text += " sztuka\n";
                    else if (items[i] <= 4)
                        ScannedInRoomAmount.Text += " sztuki\n";
                    else
                        ScannedInRoomAmount.Text += " sztuk\n";
                }
            }
            if (ScannedInRoomLabel.Text == "")
                ScannedInRoomLabel.Text = "Brak";
            else
            {
                ScannedInRoomLabel.Text = ScannedInRoomLabel.Text.Substring(0, ScannedInRoomLabel.Text.Length - 1);
                ScannedInRoomAmount.Text = ScannedInRoomAmount.Text.Substring(0, ScannedInRoomAmount.Text.Length - 1);
            }

            items = new int[] { 0, 0, 0, 0, 0, 0 };//c k m p s t
            foreach (AllScaning item in allScaning)
            {
                if (!item.reportPositionPrototype.present && item.reportPositionPrototype.previous == ScanningRoom.id)
                {
                    items = CheckAmount(items, item.AssetEntity.type.id);
                }
            }
            UnscannedInRoomLabel.Text = "";
            UnscannedInRoomAmount.Text = "";
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] > 0)
                {
                    UnscannedInRoomLabel.Text += types[i] + "\n";
                    UnscannedInRoomAmount.Text += items[i];
                    if (items[i] == 1)
                        UnscannedInRoomAmount.Text += " sztuka\n";
                    else if (items[i] <= 4)
                        UnscannedInRoomAmount.Text += " sztuki\n";
                    else
                        UnscannedInRoomAmount.Text += " sztuk\n";

                }
            }
            if (UnscannedInRoomLabel.Text == "")
                UnscannedInRoomLabel.Text = "Brak";
            else
            {
                UnscannedInRoomLabel.Text = UnscannedInRoomLabel.Text.Substring(0, UnscannedInRoomLabel.Text.Length - 1);
                UnscannedInRoomAmount.Text = UnscannedInRoomAmount.Text.Substring(0, UnscannedInRoomAmount.Text.Length - 1);
            }

            items = new int[] { 0, 0, 0, 0, 0, 0 };//c k m p s t
            foreach (AllScaning item in allScaning)
            {
                if (!item.reportPositionPrototype.present && item.reportPositionPrototype.previous != ScanningRoom.id)
                {
                    items = CheckAmount(items, item.AssetEntity.type.id);
                }
            }
            OtherLabel.Text = "";
            OtherAmount.Text = "";
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] > 0)
                {
                    OtherLabel.Text += types[i] + "\n";
                    OtherAmount.Text += items[i];
                    if (items[i] == 1)
                        OtherAmount.Text += " sztuka\n";
                    else if (items[i] <= 4)
                        OtherAmount.Text += " sztuki\n";
                    else
                        OtherAmount.Text += " sztuk\n";

                }
            }
            if (OtherLabel.Text == "")
                OtherLabel.Text = "Brak";
            else
            {
                OtherLabel.Text = OtherLabel.Text.Substring(0, OtherLabel.Text.Length - 1);
                OtherAmount.Text = OtherAmount.Text.Substring(0, OtherAmount.Text.Length - 1);
            }

            List<AllScaning> scannedItems = new List<AllScaning>();
            foreach (AllScaning item in allScaning)
            {
                if (!item.Approved && item.reportPositionPrototype.previous != ScanningRoom.id)
                {
                    scannedItems.Add(item);
                }
            }
            ReportList.ItemsSource = scannedItems;
            if (scannedItems.Count == 0)
                ButtonMoveAll.IsVisible = false;
            else
                ButtonMoveAll.IsVisible = true;
        }
        
        async private void ScannedInRoomDetails(object sender, EventArgs e)
        {
            string text = "";
            foreach (AllScaning item in allScaning)
            {
                if (item.reportPositionPrototype.present)
                {
                    text += item.AssetEntity.type.name + " numer: " + item.AssetEntity.id + "\n";
                }
            }
            if (text == "")
                text = "brak";
            await DisplayAlert("Zeskanowane z sali " + ScanningRoom.name, text, "Ok");
        }

        async private void UnscannedInRoomDetails(object sender, EventArgs e)
        {
            string text = "";
            foreach (AllScaning item in allScaning)
            {
                if (!item.reportPositionPrototype.present && item.reportPositionPrototype.previous == ScanningRoom.id)
                {
                    text += item.AssetEntity.type.name + " numer: " + item.AssetEntity.id + "\n";
                }
            }
            if (text == "")
                text = "brak";
            await DisplayAlert("Niezeskanowane z sali " + ScanningRoom.name, text, "Ok");
        }

        async private void OtherDetails(object sender, EventArgs e)
        {
            string text = "";
            foreach (AllScaning item in allScaning)
            {
                if (!item.reportPositionPrototype.present && item.reportPositionPrototype.previous != ScanningRoom.id)
                {
                    text += item.AssetEntity.type.name + " numer: " + item.AssetEntity.id + "\n";
                }
            }
            if (text == "")
                text = "brak";
            await DisplayAlert("Nieprzeiesione z sali " + ScanningRoom.name, text, "Ok");
        }

        private int[] CheckAmount(int[] items, int typeId)
        {
            switch (typeId)
            {
                case 1: items[0]++; break;
                case 2: items[1]++; break;
                case 3: items[2]++; break;
                case 4: items[3]++; break;
                case 5: items[4]++; break;
                case 6: items[5]++; break;
            }
            return items;
        }

        async private void EndScanning(object sender, EventArgs e)
        {
            bool message1 = false;
            bool message2 = false;
            foreach (AllScaning item in allScaning)
            {
                if (!item.Approved)
                {
                    if(item.AssetRoom == ScanningRoom.id)
                        message2 = true;
                    else
                        message1 = true;
                }
            }
            if (message1 == true)
            {
                await DisplayAlert("Uwaga", "Istnieją niezatwierdzone przedmioty", "Wróć");
                return;
            }
            else if (message2 == true)
            {
                bool response = await DisplayAlert("Uwaga", "Jeśli kontynuujesz, wszystkie niezeskanowane przedmioty z sali zostaną z niej usunięte!", "Kontunuuj", "Anuluj");
                if(response)
                    GenerateRaport();
                else return;
            }
            GenerateRaport();
        }

        async private void ChangeRoom(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Uwaga", "Czy na pewno chcesz przenieść tutaj ten przedmiot?", "Tak", "Nie");

            if (response)
            {
                Button button = sender as Button;
                int id = Convert.ToInt32(button.CommandParameter);
                foreach (AllScaning item in allScaning)
                {
                    if (item.ScannedId == id)
                    {
                        item.ItemMoved();
                        ShowInfo();
                        break;
                    }
                }
            }
        }

        async private void NoChange(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Uwaga", "Czy na pewno nie chcesz zmieniać lokalizacji tego przedmiotu?", "Tak", "Nie");

            if (response)
            {
                Button button = sender as Button;
                int id = Convert.ToInt32(button.CommandParameter);
                foreach (AllScaning item in allScaning)
                {
                    if (item.ScannedId == id)
                    {
                        item.Approved = true;
                        ShowInfo();
                        break;
                    }
                }
            }
        }

        async private void moveAllItems(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Uwaga", "Czy na pewno chcesz przenieść wszystkie przedmioty?", "Tak", "Nie");

            if (response)
            {
                foreach (AllScaning item in allScaning)
                {
                    if (!item.Approved && item.AssetRoom != ScanningRoom.id)
                    {
                        item.ItemMoved();
                    }
                }
                ShowInfo();
                scrollView.IsEnabled = false;
                await scrollView.ScrollToAsync(0, 0, true);
            }
        }

        async private void MoveAllInRoom(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Uwaga", "Czy na pewno chcesz przenieść wszystkie przedmioty?", "Tak", "Nie");

            if (response)
            {
                foreach (AllScaning item in allScaning)
                {
                    if (!item.Approved && item.AssetRoom == ScanningRoom.id)
                    {
                        item.ItemMoved();
                    }
                }
                ShowInfo();
            }
        }

        private async void GenerateRaport()
        {
            EnableView(false);
            ReportPositionPrototype[] reportPositionPrototype = new ReportPositionPrototype[allScaning.Count];
            for (int i = 0; i < allScaning.Count; i++) 
            {
                reportPositionPrototype[i] = allScaning[i].reportPositionPrototype;
            }
            
            ReportPrototype reportPrototype = new ReportPrototype("Raport " + ScanningRoom.building.name, ScanningRoom, reportPositionPrototype);
            int end = await api.createReport(reportPrototype);
            EnableView(true);
            if (end != -1)
            {
                App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
            }
            else
            {
                await DisplayAlert("Błąd", "Nie udało się utworzyć raportu", "Ok");
            }
        }

        private void EnableView(bool state)
        {
            LoadingScreen.IsVisible = !state;
            ButtonPrevPage.IsEnabled = state;
            ButtonConfirm.IsEnabled = state;
        }

        private async void RetPrevPage(object sender, EventArgs e)
        {
            EnableView(false);
            await Navigation.PopModalAsync();
            EnableView(true);
        }
    }
}
