using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanItemPage : ContentPage
    {
        private ZXing.Result prev=null;

        public ScanItemPage()
        {
            InitializeComponent();

            var zXingOptions = new MobileBarcodeScanningOptions()
            {
                DelayBetweenContinuousScans = 1000, // msec
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _scanner.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            _scanner.IsScanning = false;
            base.OnDisappearing();
        }

        async private void Cancel(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Anulować skanowanie?", "Czy na pewno chcesz anulować skanowanie?", "Tak", "Nie");

            if(response)
            {
                await Navigation.PopAsync();
            }
        }

        private void EndScanning(object sender, EventArgs e)
        {

        }

        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            if(prev == null || result.Text!=prev.Text)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    prev = result;
                    await DisplayAlert("Wynik skanowania", result.Text, "OK");
                });
            }
        }
    }
}