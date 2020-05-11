using System;

namespace Inwentaryzacja.models
{
	public class ScanningPosition
	{
        public Asset Asset { get; private set; }
        public bool Present { get; private set; }
        public Room PreviusRoom { get; private set; }

        public ScanningPosition(Asset asset, Room previusRoom, bool present)
		{
            Asset = asset;
            Present = present;
            PreviusRoom = previusRoom;
        }
	}
}

