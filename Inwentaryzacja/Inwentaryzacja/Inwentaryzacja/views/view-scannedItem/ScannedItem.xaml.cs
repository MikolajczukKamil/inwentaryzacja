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
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        APIController api;
        RoomEntity ScanningRoom;
        List<AllScaning> allScaning;
        public ScannedItem(List<AllScaning> scannedItems, RoomEntity scanningRoom)
        {
            InitializeComponent();
            ReportList.ItemsSource = scannedItems;
            allScaning = scannedItems;
            api = new APIController();
            ScanningRoom = scanningRoom;
            BindingContext = this;
        }

        public class AllScaning
        {
            public ReportPositionPrototype reportPositionPrototype;

            private AssetEntity AssetEntity;
            private RoomEntity ScanningRoom;
            public bool IsScanned;
            private bool WrongRoom = true;
            private int? AssetRoom = null;

            public AllScaning(AssetEntity assetEntity, RoomEntity assetRoom, RoomEntity scanningRoom, bool isScanned)
            {
                AssetType assetType = new AssetType(assetEntity.type.id, assetEntity.type.name, assetEntity.type.letter);
                Asset asset = new Asset(assetEntity.id, assetType);
                Room room = null;
                ButtonsViews = true;
                if (assetRoom != null)
                {
                    Building building = new Building(assetRoom.building.id, assetRoom.building.name);
                    room = new Room(assetRoom.id, assetRoom.name, building);
                    AssetRoom = assetRoom.id;
                    if (assetRoom.id == scanningRoom.id)
                    {
                        ButtonsViews = false;
                        WrongRoom = false;
                    }
                }
                reportPositionPrototype = new ReportPositionPrototype(asset, room, isScanned);
                AssetEntity = assetEntity;
                IsScanned = isScanned;
                ScannedId = assetEntity.id;
                ScanningRoom = scanningRoom;
            }
            public void Scanned()
            {
                IsScanned = true;
                reportPositionPrototype.present = true;
                WrongRoom = false;
                AssetRoom = ScanningRoom.id;
            }

            public string ScaningText { get { return string.Format("{0} {1}", AssetEntity.type.name, AssetEntity.type.id); } }
            public string RoomText { get { return string.Format("Id: {0}{1}", AssetEntity.id, AssetRoom != null ? "\nSala: " + AssetRoom : ", Nie należy do żadnej sali"); } } 
            public string PictureUrl { get { return WrongRoom ? "No.png" : "Yes.png"; } }
            public bool ScannedText { get { return (IsScanned && !ButtonsViews) ? true : false; } }
            public bool ButtonsViews { get; set; }
            public int ScannedId { get; set; }

        }

        async private void EndScanning(object sender, EventArgs e)
        {
            bool message = false;
            foreach (AllScaning item in ReportList.ItemsSource)
            {
                if (!item.IsScanned || item.ButtonsViews) 
                {
                    message = true;
                    break;
                }
            }
            if (message == true)
            {
                bool response = await DisplayAlert("Uwaga", "Istnieją niezatwierdzone lub niezeskanowane przedmioty", "Kontynuuj", "Wróć");
                if (response)
                    GenerateRaport();
                return;
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
                        item.ButtonsViews = false;
                        item.Scanned();
                        await DisplayAlert("Uwaga", "Przycisk zadziałał, ale widok się nie odświeżył", "Ok");
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
                        item.ButtonsViews = false;
                        await DisplayAlert("Uwaga", "Przycisk zadziałał, ale widok się nie odświeżył", "Ok");
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
                    if (item.ButtonsViews)
                    {
                        item.ButtonsViews = false;
                        item.Scanned();
                        break;
                    }
                }
                await DisplayAlert("Uwaga", "Przycisk zadziałał, ale widok się nie odświeżył", "Ok");
            }
        }

        private async void GenerateRaport()
        {
            List<AllScaning> scanningCopy = new List<AllScaning>();
            foreach (AllScaning item in allScaning)
            {
                if(item.IsScanned)
                {
                    scanningCopy.Add(item);
                }
            }

            ReportPositionPrototype[] reportPositionPrototype = new ReportPositionPrototype[scanningCopy.Count];
            for (int i = 0; i < scanningCopy.Count; i++) 
            {
                reportPositionPrototype[i] = scanningCopy[i].reportPositionPrototype;
            }
            Room roomEntity = new Room(ScanningRoom.id, ScanningRoom.name, new Building(ScanningRoom.building.id, ScanningRoom.building.name));
            
            ReportPrototype reportPrototype = new ReportPrototype("Raport sala "+ScanningRoom.name, roomEntity, reportPositionPrototype);
            EnableView(false);
            bool end = await api.createReport(reportPrototype);
            EnableView(true);
            if (end)
            {
                App.Current.MainPage = new NavigationPage(new AllReportsPage());
            }
            else
            {
                await DisplayAlert("Błąd", "Nie udało się utworzyć raportu :(", "Ok");
            }
        }

        private void EnableView(bool state)
        {
            LoadingScreen.IsVisible = !state;
            Button1.IsEnabled = state;
            Button2.IsEnabled = state;
            Button3.IsEnabled = state;
        }

        private async void RetPrevPage(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}