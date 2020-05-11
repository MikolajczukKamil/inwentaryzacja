using System;

namespace Inwentaryzacja.Models
{
	public class RoomPropotype
	{
		public string name;
		public Building building;

		public RoomPropotype(string name, Building building)
		{
			this.name = name;
			this.building = building;
		}
	}
}
