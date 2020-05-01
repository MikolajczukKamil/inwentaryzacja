using System;

namespace Inwentaryzacja.models
{
	public class ScanningPosition
	{
		private Room _room;
		private ScanningPropotype _scanningPropotype;
		private Asset _asset;
		private Scanning _scanning;

		public Asset Thing => _asset;
		public Room PreviusRoom => _room;
		public bool MoveHere;

		public ScanningPosition(Room preRoom, Asset asset, ScanningPropotype scanningPropotype, Scanning scanning, bool moveHere)
		{
			_room = preRoom;
			_scanning = scanning;
			_scanningPropotype = scanningPropotype;
			_asset = asset;
			MoveHere = moveHere;
		}
	}
}
