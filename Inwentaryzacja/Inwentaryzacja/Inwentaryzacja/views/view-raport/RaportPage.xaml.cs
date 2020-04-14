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
using Inwentaryzacja.view_models;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaportPage : ContentPage
    {
        ItemViewModel it;
        public RaportPage()
        {
            InitializeComponent();
            it = new ItemViewModel();
            ItemsList1.ItemsSource = it.ListItems;
        }
    }
}