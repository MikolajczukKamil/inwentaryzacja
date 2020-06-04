using System;

namespace Inwentaryzacja.Models
{
	public class ScanningPosition
	{
        public Asset Asset { get; private set; }
        public bool Present { get; private set; }
        public Room Previus { get; private set; }

        public ScanningPosition(Asset asset, Room previus, bool present)
		{
            Asset = asset;
            Present = present;
            Previus = previus;
        }

        public ScanPositionPropotype GeneratePrototype()
        {
            throw new System.Exception("Not implemented");
        }
	}
}

