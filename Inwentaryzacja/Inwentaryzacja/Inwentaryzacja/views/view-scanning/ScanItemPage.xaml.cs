using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing;
using System.Threading;
using Inwentaryzacja.views.view_scannedItem;
using Inwentaryzacja.views;
using Inwentaryzacja.Models;
using Xamarin.Essentials;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.views.Helpers;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// Klasa odpowiadajaca za widok okna skanowania przedmiotu w danym pokoju
    /// </summary>
    public partial class ScanItemPage : ContentPage
    {
        private APIController api = new APIController();
        private RoomEntity Room;
        private Result previus = null;
        private List<string> scannedItem = new List<string>();
        private List<ScanPosition> AllPositions = new List<ScanPosition>();

        private ScanningUpdate scanningUpdate;

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="room">pokoj w ktorym odbywa sie skanowanie</param>
        public ScanItemPage(RoomEntity room, int scanId, ScanEntity previusScan)
        {
            Room = room;

            scanningUpdate = new ScanningUpdate(api, room, scanId);

            InitializeComponent();
            GetAllAssets();

            if(previusScan != null)
            {
                InitializeWith(previusScan);
            }

            _scanner.Options = new MobileBarcodeScanningOptions()
            {
                DelayBetweenContinuousScans = 1800, // msec
                UseFrontCameraIfAvailable = false,
                PossibleFormats = new List<BarcodeFormat>(new[]
                {
                     BarcodeFormat.EAN_8,
                     BarcodeFormat.EAN_13,
                     BarcodeFormat.CODE_128,
                     BarcodeFormat.QR_CODE
                }),
                TryHarder = false //Gets or sets a flag which cause a deeper look into the bitmap.
            };
        }

        /// <summary>
        /// Funkcja odpowiadajaca za inicjalizację skanowania z wykorzystaniem poprzedniego składowania
        /// </summary>
        async void InitializeWith(ScanEntity previusScan)
        {
            var positions = await api.GetScanPositions(previusScan.id);

            if (positions == null) return;

            foreach (var position in positions)
            {
                scannedItem.Add($"{position.asset.type.id}-{position.asset.id}");

                ScanAsset(position.asset);

                var localReprezentation = AllPositions.Find(el => el.AssetEntity.id == position.asset.id);

                switch(position.state)
                {
                    case 0:
                        // po prostu zeskanowano
                        break;
                    case 1:
                        // zaakceptowano
                        localReprezentation.ItemMoved();
                        break;
                    case 2:
                        // usunięto
                        localReprezentation.ItemDontMove();
                        break;
                }
            }

            UpdateCounter();
        }

        /// <summary>
        /// Funkcja odpowiadajaca za zwrocenie wszystkich srodkow trwalych z danego pokoju
        /// </summary>
        async void GetAllAssets()
        {
            AssetEntity[] assetEntity = await api.getAssetsInRoom(Room.id);

            if (assetEntity == null) return;

            foreach (var item in assetEntity)
            {
                AllPositions.Add(new ScanPosition(item, Room, Room));
            }
        }

        /// <summary>
        /// Funkcja odpowiadajaca za wyswietlenie okna skanowania po jego zaladowaniu
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _scanner.IsScanning = true;
        }

        /// <summary>
        /// Funkcja odpowiadajaca za zamkniecie okna skanowania
        /// </summary>
        protected override void OnDisappearing()
        {
            //_scanner.IsScanning = false;
            base.OnDisappearing();
        }

        /// <summary>
        /// Funkcja odpowiadajaca za anulowanie sesji skanowania
        /// </summary>
        async private void Cancel(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Anulować skanowanie?", "Czy na pewno chcesz anulować skanowanie?", "Tak", "Nie");

            if (response)
            {
                await Navigation.PopModalAsync();
            }
        }

        /// <summary>
        /// Funkcja odpowiadajaca za pokazanie zeskanowanego przedmiotu 
        /// </summary>
        private async void ShowScanedItem(object sender, EventArgs e)
        {
            PreviewButton.IsEnabled = false;
            await Navigation.PushModalAsync(new ScannedItem(AllPositions, Room, scanningUpdate), true);
            PreviewButton.IsEnabled = true;
        }

        /// <summary>
        /// Funkcja odpowiadajaca za wyswietlenie informacji ze dany srodek trwaly zostal zeskanowany
        /// </summary>
        private async Task ShowPopup(string message = "Zeskanowano!")
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _contentPopup.IsVisible = true;
                });
            });

            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _contentPopup.Text = message;
                });
            });

            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _backColorPopup.WidthRequest = _contentPopup.Width;
                });
            });

            await _popup.FadeTo(1, 150);
            await Task.Delay(600);
            await _popup.FadeTo(0, 400);


            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _contentPopup.IsVisible = false;
                });
            });

        }

        /// <summary>
        /// Funkcja odpowiadajaca za skanowanie danego srodka trwalego
        /// </summary>
        private async void ZXingScannerView_OnScanResult(Result result)
        {
            if (previus != null && (result.Text == previus.Text || ListContainItem(result.Text)))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ShowPopup("Już zeskanowano ten przedmiot!");
                });

                return;
            }

            string[] positions;
            int AssetId;

            try
            {
                positions = result.Text.Split('-');
                AssetId = Convert.ToInt32(positions[1]);
            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ShowPopup("Zły format kodu");
                });

                return;
            }

            AssetInfoEntity assetInfo = await api.getAssetInfo(AssetId);

            if (assetInfo == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ShowPopup("Nieznany obiekt");
                });

                return;
            }

            ScanAsset(assetInfo);

            scanningUpdate.Update(AllPositions);

            scannedItem.Add(result.Text);

            previus = result;

            UpdateCounter();
        }

        private void UpdateCounter()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _infoLabel.Text = $"Liczba zeskanowanych przedmiotów: {scannedItem.Count}";

                Vibration.Vibrate(TimeSpan.FromMilliseconds(100));
            });
        }

        private void ScanAsset(AssetInfoEntity assetInfo)
        {
            try
            {
                if (assetInfo.room == null || assetInfo.room.id != Room.id)
                {
                    // Nowy asset

                    AllPositions.Add(new ScanPosition(assetInfo, assetInfo.room, Room));

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ShowPopup("Zeskanowano przedmiot z innej sali");
                    });
                }
                else
                {
                    // Zapisz jako zeskanowany

                    AllPositions.Find(x => x.ScannedId == assetInfo.id).ItemMoved();

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ShowPopup();
                    });
                }
            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ShowPopup("Wystąpił błąd");
                });

                return;
            }
        }

        /// <summary>
        /// Funkcja odpowiadajaca za liste zeskanowanych srodkow trwalych
        /// </summary>
        private bool ListContainItem(string text)
        {
            foreach (var item in scannedItem)
            {
                if (item == text)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Funkcja odpowiadajaca za wlaczenie latarki/flasha w skanerze (telefonie)
        /// </summary>
        private void TurnLight(object sender, EventArgs e)
        {
            try
            {
                _scanner.ToggleTorch();
            }
            catch (Exception)
            {
            }
        }
        
        /// <summary>
        /// Funkcja odpowiadajaca za obsluge przycisku powrotu
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            Cancel(this, null);

            return true;
        }
    }
}
