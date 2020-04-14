using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja
{
    public class Item
    {
        public string Name { get; set; }
        public int ID_items { get; set; }
        public int Room { get; set; }
        public string UrlPicture { get; set; }

        public List<Item> GetItem()
        {
            List<Item> items = new List<Item>()
            {
                new Item(){Name="Krzesło",ID_items=12334,Room=12,UrlPicture="Yes.jpg"},
                new Item(){Name="Krzesło",ID_items=33334,Room=3,UrlPicture="No.jpg"},
                new Item(){Name="Krzesło",ID_items=111334,Room=2,UrlPicture="No.jpg"},
                new Item(){Name="Stół",ID_items=34134,Room=41,UrlPicture="Yes.jpg"},
                new Item(){Name="Tablica",ID_items=997,Room=12,UrlPicture="Yes.jpg"},
                new Item(){Name="Stół",ID_items=33334,Room=121,UrlPicture="Yes.jpg"},
                new Item(){Name="Krzesło",ID_items=12334,Room=1,UrlPicture="Yes.jpg"},
                new Item(){Name="Stół",ID_items=0700,Room=16,UrlPicture="No.jpg"}
            };
            return items;

        }
        public string NewItem
        {

            get
            {
                return string.Format("{0} (id:{1}", Name, ID_items);
            }
        }
    }
    public class ItemViewModel
    {
        public List<Item> ListItems { get; set; }
        public ItemViewModel()
        {
            ListItems = new Item().GetItem();
        }
    }
}
