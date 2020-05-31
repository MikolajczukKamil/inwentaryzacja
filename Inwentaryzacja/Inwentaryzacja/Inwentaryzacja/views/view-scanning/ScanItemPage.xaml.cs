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
        APIController api;
        private RoomEntity Room;
        private ZXing.Result prev=null;
        private List<string> scannedItem = new List<string>();
        private List<AllScaning> AllItems = new List<AllScaning>();

        public ScanItemPage(RoomEntity room)
        {
            Room = room;

            InitializeComponent();
            api = new APIController();
            GetAllAssets();

            var zXingOptions = new MobileBarcodeScanningOptions()
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
            _scanner.Options = zXingOptions;
        }

        async void GetAllAssets()
        {
            AssetEntity[] assetEntity = await api.getAssetsInRoom(Room.id);
            foreach (var item in assetEntity)
            {
                AllItems.Add(new AllScaning(item, Room, Room, false));
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

            if(response)
            {
                App.Current.MainPage = new ChooseRoomPage();
            }
        }

        private async void ShowScanedItem(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScannedItem(AllItems, Room), true);
            //App.Current.MainPage = new NavigationPage(new ScannedItem(AllItems));
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


        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            if(prev == null || result.Text!=prev.Text)
            {
                if(!ListContainItem(result.Text))
                {
					
                    try
                    {
                        string[] positions = result.Text.Split('-');
                        AssetInfoEntity assetInfoEntity = api.getAssetInfo(Convert.ToInt32(positions[1])).Result;
                        if (assetInfoEntity.room.id == Room.id)
                        {
                            AllItems.Find(x => x.ScannedId == assetInfoEntity.id).Scanned();
                        }
                        else
                        {
                            AllItems.Add(new AllScaning(assetInfoEntity, assetInfoEntity.room, Room, true));
                        }
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            prev = result;
                            _infoLabel.Text = "Liczba zeskanowanych przedmiotów: " + scannedItem.Count;
							Vibration.Vibrate(TimeSpan.FromMilliseconds(100));
                            await ShowPopup();

                            //await DisplayAlert("Wynik skanowania", result.Text, "OK");
                        });
                        scannedItem.Add(result.Text);
                    }
                    catch (Exception)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await ShowPopup("Zły format kodu"); ;
                        });
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ShowPopup("Już zeskanowano ten przedmiot!");
                    });
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ShowPopup("Już zeskanowano ten przedmiot!");
                });
            }
        }

        private bool ListContainItem(string text)
        {
            foreach (var item in scannedItem)
            {
                if(item==text)
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
    }
}