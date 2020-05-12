using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    public class ScanningPositionPropotype
    {
        public int asset;
        public int previus;
        public bool present;

        public ScanningPositionPropotype(int asset, int previus, bool present)
        {
            this.asset = asset;
            this.previus = previus;
            this.present = present;
        }
    }
}
