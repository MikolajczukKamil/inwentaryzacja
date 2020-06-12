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
using static Inwentaryzacja.views.view_scannedItem.ScannedItem;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanItemPage : ContentPage
    {
        private APIController api = new APIController();
        private RoomEntity Room;
        private Result prev = null;
        private List<string> scannedItem = new List<string>();
        private List<AllScaning> AllItems = new List<AllScaning>();

        private ScanningUpdate scanningUpdate;

        public ScanItemPage(RoomEntity room, int scanId = 1)
        {
            Room = room;

            scanningUpdate = new ScanningUpdate(api, room, scanId);

            InitializeComponent();
            GetAllAssets();

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

        async void GetAllAssets()
        {
            AssetEntity[] assetEntity = await api.getAssetsInRoom(Room.id);

            if (assetEntity == null) return;

            foreach (var item in assetEntity)
            {
                AllItems.Add(new AllScaning(item, Room, Room));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _scanner.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            //_scanner.IsScanning = false;
            base.OnDisappearing();
        }

        async private void Cancel(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Anulować skanowanie?", "Czy na pewno chcesz anulować skanowanie?", "Tak", "Nie");

            if (response)
            {
                await Navigation.PopModalAsync();
            }
        }

        private async void ShowScanedItem(object sender, EventArgs e)
        {
            PreviewButton.IsEnabled = false;
            await Navigation.PushModalAsync(new ScannedItem(AllItems, Room), true);
            PreviewButton.IsEnabled = true;
        }

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

        private async void ZXingScannerView_OnScanResult(Result result)
        {
            if (prev != null && (result.Text == prev.Text || ListContainItem(result.Text)))
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

            scanningUpdate.Update(AllItems);

            scannedItem.Add(result.Text);

            prev = result;

            Device.BeginInvokeOnMainThread(() =>
            {
                _infoLabel.Text = "Liczba zeskanowanych przedmiotów: " + scannedItem.Count;
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

                    AllItems.Add(new AllScaning(assetInfo, assetInfo.room, Room));

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ShowPopup("Zeskanowano przedmiot z innej sali");
                    });
                }
                else
                {
                    // Zapisz jako zeskanowany

                    AllItems.Find(x => x.ScannedId == assetInfo.id).ItemMoved();

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

        protected override bool OnBackButtonPressed()
        {
            Cancel(this, null);

            return true;
        }
    }
}
