using Inwentaryzacja.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    public class ReportPositionPrototype
    {
        public int id;
        public int? previous = null;
        public bool present;

        public ReportPositionPrototype(AssetEntity asset, RoomEntity previous, bool present)
        {
            this.id = asset.id;
            if (previous != null)
                this.previous = previous.id;
            this.present = present;
        }
    }
}
