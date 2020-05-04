using System;

namespace Inwentaryzacja.models
{
	public class RoomPropotype
	{
		public string name { get; set; }
		public int building { get; set; }

		public RoomPropotype(string name, int building)
		{
			this.name = name;
			this.building = building;
		}
	}
}
