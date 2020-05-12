using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    public class ReportPositionPrototype
    {
        public int id;
        public int previous;
        public bool present;

        public ReportPositionPrototype(Asset asset, Room previous, bool present)
        {
            this.id = asset.Id;
            this.previous = previous == null ? -1 : previous.Id;
            this.present = present;
        }
    }
}
