using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    public class ScanUpdatePropotype
    {
        public int id;
        
        public ScanPositionPropotype[] positions;

        public ScanUpdatePropotype(int id, ScanPositionPropotype[] positions)
        {
            this.id = id;
            this.positions = positions;
        }
    }

    public class ScanPositionPropotype
    {
        public int asset;
        public int state;

        public ScanPositionPropotype(int asset, int state)
        {
            this.asset = asset;
            this.state = state;
        }
    }
}
