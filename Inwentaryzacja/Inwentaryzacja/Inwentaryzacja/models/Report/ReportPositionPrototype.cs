using Inwentaryzacja.Controllers.Api;
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

        public ReportPositionPrototype(AssetEntity asset, RoomEntity previous, bool present)
        {
            this.id = asset.id;
            this.previous = previous == null ? -1 : previous.id;
            this.present = present;
        }
    }
}
