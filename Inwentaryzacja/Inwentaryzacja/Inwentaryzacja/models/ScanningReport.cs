using System;

namespace Inwentaryzacja.models
{
	public class ScanningReport
	{
		private Room _room;
		private ScanningPosition[] _positions;

		public Room Room => _room;
		public ScanningPosition[] Positions => _positions;

		public ScanningReport(Room room, ScanningPosition[] positions)
		{
			_room = room;
			_positions = positions;
		}
	}
}
