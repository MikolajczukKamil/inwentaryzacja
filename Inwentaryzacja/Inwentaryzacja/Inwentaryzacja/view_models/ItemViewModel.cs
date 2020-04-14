using Inwentaryzacja.models;
using System.Collections.Generic;

namespace Inwentaryzacja.view_models
{
    public class ItemViewModel
    {
        public List<RaportItem> ListItems { get; set; }
        public ItemViewModel()
        {
            ListItems = createMockItems();
        }

        private List<RaportItem> createMockItems()
        {
            List<RaportItem> items = new List<RaportItem>()
            {
                new RaportItem ("Krzesło",12334,12,"Yes.jpg"),
                new RaportItem ("Krzesło",33334,3,"No.jpg"),
                new RaportItem ("Krzesło",111334,2,"No.jpg"),
                new RaportItem ("Stół",34134,41,"Yes.jpg"),
                new RaportItem ("Tablica",997,12,"Yes.jpg"),
                new RaportItem ("Stół",33334,121,"Yes.jpg"),
                new RaportItem ("Krzesło",12334,1,"Yes.jpg"),
                new RaportItem ("Stół",0700,16,"No.jpg")
            };
            return items;
        }
    }
}
