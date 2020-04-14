using Inwentaryzacja.models;
using System.Collections.Generic;

namespace Inwentaryzacja.view_models
{
    public class ItemViewModel
    {
        public List<Item> ListItems { get; set; }
        public ItemViewModel()
        {
            ListItems = createMockItems();
        }

        private List<Item> createMockItems()
        {
            List<Item> items = new List<Item>()
            {
                new Item ("Krzesło",12334,12,"Yes.jpg"),
                new Item ("Krzesło",33334,3,"No.jpg"),
                new Item ("Krzesło",111334,2,"No.jpg"),
                new Item ("Stół",34134,41,"Yes.jpg"),
                new Item ("Tablica",997,12,"Yes.jpg"),
                new Item ("Stół",33334,121,"Yes.jpg"),
                new Item ("Krzesło",12334,1,"Yes.jpg"),
                new Item ("Stół",0700,16,"No.jpg")
            };
            return items;
        }
    }
}
