using System;

namespace Inwentaryzacja.Models
{
	public class ScanningPropotype
	{
		public int id;
		public int room;
		public ScanningPositionPropotype[] assets;

		public ScanningPropotype(Room room, ScanningPositionPropotype[] positions, int id = -1)
		{
			this.id = id;
			this.room = room.Id;
			this.assets = positions;
		}
	}
}
