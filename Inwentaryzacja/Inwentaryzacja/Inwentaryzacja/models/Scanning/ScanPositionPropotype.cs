using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    public class ScanPositionPropotype
    {
        public int id;
        
        public PositionPropotype[] positions;

        public ScanPositionPropotype(int id, PositionPropotype[] positions)
        {
            this.id = id;
            this.positions = positions;
        }
    }

    public class PositionPropotype
    {
        public int asset;
        public int state;

        public PositionPropotype(int asset, int state)
        {
            this.asset = asset;
            this.state = state;
        }
    }
}
