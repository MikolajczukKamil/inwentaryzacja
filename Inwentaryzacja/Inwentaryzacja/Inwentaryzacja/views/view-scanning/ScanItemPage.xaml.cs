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
    /// <summary>
    /// Klasa odpowiadajaca za widok okna skanowania przedmiotu w danym pokoju
    /// </summary>
    public partial class ScanItemPage : ContentPage
    {
        APIController api;
        private RoomEntity Room;
        private ZXing.Result prev = null;
        private List<string> scannedItem = new List<string>();
        private List<AllScaning> AllItems = new List<AllScaning>();

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="room">pokoj w ktorym odbywa sie skanowanie</param>
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
        /// <summary>
        /// Funkcja odpowiadajaca za zwrocenie wszystkich srodkow trwalych z danego pokoju
        /// </summary>
        async void GetAllAssets()
        {
            AssetEntity[] assetEntity = await api.getAssetsInRoom(Room.id);
            foreach (var item in assetEntity)
            {
                AllItems.Add(new AllScaning(item, Room, Room));
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
            await Navigation.PushModalAsync(new ScannedItem(AllItems, Room), true);
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
        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            if (prev == null || result.Text != prev.Text)
            {
                if (!ListContainItem(result.Text))
                {
                    string[] positions;
                    int TypeID; 
                    int AssetId; 
                    AssetInfoEntity assetInfoEntity; 
                    try
                    {
                        positions = result.Text.Split('-');
                        TypeID = Convert.ToInt32(positions[0]);
                        AssetId = Convert.ToInt32(positions[1]);
                    }
                    catch (Exception)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            positions = result.Text.Split('-');
                            await ShowPopup("Zły format kodu");
                        });
                        return;
                    }
                    assetInfoEntity = api.getAssetInfo(AssetId).Result;
                    if(assetInfoEntity != null)
                    {
                        try
                        {
                            if (assetInfoEntity.room == null || assetInfoEntity.room.id != Room.id)
                            {
                                AllItems.Add(new AllScaning(assetInfoEntity, assetInfoEntity.room, Room));
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await ShowPopup("Zeskanowano przedmiot z innej sali");
                                });
                            }
                            else
                            {
                                AllItems.Find(x => x.ScannedId == assetInfoEntity.id).ItemMoved();
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await ShowPopup(); ;
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
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await ShowPopup("Nieznany obiekt");
                            //TU JEST DODAWANIE NOWEGO PRZEDMIOTU DO BAZY DANYCH, KTÓRE NIE MOŻE BYĆ ZREALIZOWANE
                            /*
                            bool response = await DisplayAlert("Nieznany przedmiot", "Wyktyro nowy obiekt. Czy chcesz dodać go do bazy danych?", "Tak", "Nie");
                            if (response)
                            {
                                try
                                {
                                    AssetType at = null;
                                    switch (positions[0])
                                    {
                                        case "c":
                                            at = new AssetType(1, "komputer", 'c');
                                            break;
                                        case "k":
                                            at = new AssetType(2, "krzesło", 'k');
                                            break;
                                        case "m":
                                            at = new AssetType(3, "monitor", 'm');
                                            break;
                                        case "p":
                                            at = new AssetType(4, "projektor", 'p');
                                            break;
                                        case "s":
                                            at = new AssetType(5, "stół", 's');
                                            break;
                                        case "t":
                                            at = new AssetType(6, "tablica", 't');
                                            break;
                                        default:
                                            throw new Exception();
                                    }
                                    prev = result;
                                    AssetPrototype ap = new AssetPrototype(at);
                                    bool check = await api.CreateAsset(ap);
                                    if (check)
                                    {
                                        assetInfoEntity = api.getAssetInfo(AssetId).Result;
                                        AllItems.Add(new AllScaning(assetInfoEntity, null, Room, true));
                                        Device.BeginInvokeOnMainThread(async () =>
                                        {
                                            await ShowPopup("Dodano nowy przedmiot");
                                        });
                                        scannedItem.Add(result.Text);
                                        _infoLabel.Text = "Liczba zeskanowanych przedmiotów: " + scannedItem.Count;
                                    }
                                    else
                                        throw new Exception();
                                }
                                catch (Exception)
                                {
                                    prev = null;
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await ShowPopup("Nie udało się dodać");
                                    });
                                }
                            }*/
                        });
                        return;
                    }

                    prev = result;
                    scannedItem.Add(result.Text);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        _infoLabel.Text = "Liczba zeskanowanych przedmiotów: " + scannedItem.Count;
                        Vibration.Vibrate(TimeSpan.FromMilliseconds(100));

                        //await DisplayAlert("Wynik skanowania", result.Text, "OK");
                    });
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
