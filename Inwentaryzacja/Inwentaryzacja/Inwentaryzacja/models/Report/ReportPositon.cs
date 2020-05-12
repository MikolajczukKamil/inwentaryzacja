using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    public class ReportPosition
    {
        public Asset Asset { get; private set; }
        public bool Present { get; private set; }
        public Room Previus { get; private set; }
        public string Label
        {
            get { return $"{Asset.Type.Name} (id: {Asset.Id})"; }
        }

        public ReportPosition(Asset asset, Room previus, bool present)
        {
            Asset = asset;
            Present = present;
            Previus = previus;
        }
    }
}
