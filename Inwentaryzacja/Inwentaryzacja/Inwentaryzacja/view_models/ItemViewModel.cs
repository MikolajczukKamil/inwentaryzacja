using Inwentaryzacja.Models;
using System.Collections.Generic;

namespace Inwentaryzacja.view_models
{
    public class ItemViewModel
    {
        public List<ReportPosition> ListItems { get; set; }
        public ItemViewModel()
        {
            ListItems = createMockItems();
        }

        private List<ReportPosition> createMockItems()
        {
            List<ReportPosition> items = new List<ReportPosition>()
            {
#if false // TODO
		                new ReportPosition ("Krzesło",12334,12,"Yes.jpg"),
                new ReportPosition ("Krzesło",33334,3,"No.jpg"),
                new ReportPosition ("Krzesło",111334,2,"No.jpg"),
                new ReportPosition ("Stół",34134,41,"Yes.jpg"),
                new ReportPosition ("Tablica",997,12,"Yes.jpg"),
                new ReportPosition ("Stół",33334,121,"Yes.jpg"),
                new ReportPosition ("Krzesło",12334,1,"Yes.jpg"),
                new ReportPosition ("Stół",0700,16,"No.jpg")  
	#endif
            };
            return items;
        }
    }
}
