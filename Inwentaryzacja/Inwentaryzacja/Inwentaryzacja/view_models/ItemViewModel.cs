using Inwentaryzacja.models;
using System.Collections.Generic;

namespace Inwentaryzacja.view_models
{
    public class ItemViewModel
    {
        public List<ReportPositon> ListItems { get; set; }
        public ItemViewModel()
        {
            ListItems = createMockItems();
        }

        private List<ReportPositon> createMockItems()
        {
            List<ReportPositon> items = new List<ReportPositon>()
            {
                new ReportPositon ("Krzesło",12334,12,"Yes.jpg"),
                new ReportPositon ("Krzesło",33334,3,"No.jpg"),
                new ReportPositon ("Krzesło",111334,2,"No.jpg"),
                new ReportPositon ("Stół",34134,41,"Yes.jpg"),
                new ReportPositon ("Tablica",997,12,"Yes.jpg"),
                new ReportPositon ("Stół",33334,121,"Yes.jpg"),
                new ReportPositon ("Krzesło",12334,1,"Yes.jpg"),
                new ReportPositon ("Stół",0700,16,"No.jpg")
            };
            return items;
        }
    }
}
