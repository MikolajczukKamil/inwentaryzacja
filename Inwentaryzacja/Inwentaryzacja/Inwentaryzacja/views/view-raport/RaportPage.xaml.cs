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


namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaportPage : ContentPage
    {
        ItemsViewModel it;
        public RaportPage()
        {
            InitializeComponent();
            it = new ItemsViewModel();
            Listap.ItemsSource = it.Items;
        }
    }
    
    public class Items
    {
        public string Nazwa { get; set; }
        public int ID_items { get; set; }
        public int Sale { get; set; }
        public string urlobrazka { get; set; }

        public List<Items> GetItems()
        {
            List<Items> itemki = new List<Items>()
            {
                new Items(){Nazwa="Krzesło",ID_items=12334,Sale=12,urlobrazka="Yes.jpg"},
                new Items(){Nazwa="Krzesło",ID_items=33334,Sale=3,urlobrazka="No.jpg"},
                new Items(){Nazwa="Krzesło",ID_items=111334,Sale=2,urlobrazka="No.jpg"},
                new Items(){Nazwa="Stół",ID_items=34134,Sale=41,urlobrazka="Yes.jpg"},
                new Items(){Nazwa="Tablica",ID_items=997,Sale=12,urlobrazka="Yes.jpg"},
                new Items(){Nazwa="Stół",ID_items=33334,Sale=121,urlobrazka="Yes.jpg"},
                new Items(){Nazwa="Krzesło",ID_items=12334,Sale=1,urlobrazka="Yes.jpg"},
                new Items(){Nazwa="Stół",ID_items=0700,Sale=16,urlobrazka="No.jpg"}
            };
            return itemki;

        }
        public string NewItem
        {

            get
            {
                return string.Format("{0} (id:{1}", Nazwa, ID_items);
            }
        }
    }
    public class ItemsViewModel
    {
        public List<Items> Items { get; set; }
        public ItemsViewModel()
        {
            Items = new Items().GetItems();
        }
    }
}