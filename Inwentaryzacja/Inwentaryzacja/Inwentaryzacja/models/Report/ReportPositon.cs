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
        public string PictureUrl { get; private set; }
        public string ItemData
        {
            get { return $"{Asset.Name} (id:{Asset.Id})"; }
        }

        public ReportPosition(Asset asset, Room previusRoom, bool present, string pictureUrl)
        {
            Asset = asset;
            Present = present;
            PreviusRoom = previusRoom;
            PictureUrl = pictureUrl;
        }

    }
}
