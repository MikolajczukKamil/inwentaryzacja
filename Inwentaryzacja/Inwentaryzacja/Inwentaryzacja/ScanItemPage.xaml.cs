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
    public partial class ScanItemPage : ContentPage
    {
        public ScanItemPage()
        {
            InitializeComponent();
        }

        async void Cancel(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void EndScanning(object sender, EventArgs e)
        {

        }
    }
}