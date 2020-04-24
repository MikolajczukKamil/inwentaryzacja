using Inwentaryzacja.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomeViewPage : ContentPage
    {
        public WelcomeViewPage()
        {
            InitializeComponent();
        }

        private async void scanButtonClicked(object sender, EventArgs e)
        {
            Asset asset = new Asset();
            asset.name = "k34";
            asset.assetType = 2;

            if(await Asset.sendAsset(asset))
            {
                await DisplayAlert("Udało się", "" , "OK");
            }
            else
            {
                await DisplayAlert("Błąd", "", "OK");
            }
            
        }
    }
}