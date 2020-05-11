using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.models
{
    public class ReportPosition
    {
        public Asset Asset { get; private set; }
        public bool Present { get; private set; }
        public Room PreviusRoom { get; private set; }
        public string Label
        {
            get { return $"{Asset.Name} (id:{Asset.Id})"; }
        }

        public ReportPosition(Asset asset, Room previusRoom, bool present)
        {
            Asset = asset;
            Present = present;
            PreviusRoom = previusRoom;
        }
    }
}
