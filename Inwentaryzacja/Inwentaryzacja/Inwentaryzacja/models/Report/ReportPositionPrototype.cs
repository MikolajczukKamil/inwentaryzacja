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

        public ReportPositionPrototype(Asset asset, bool present)
        {
            this.id = asset.Id;
            this.present = present;
        }
    }
}