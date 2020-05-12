using System;

namespace Inwentaryzacja.Models
{
	public class RoomPropotype
	{
		public string name;
		public int building;

		public RoomPropotype(string name, Building building)
		{
			this.name = name;
			this.building = building.Id;
		}
	}
}
