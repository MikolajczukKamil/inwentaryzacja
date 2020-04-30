using System;

namespace Inwentaryzacja.models
{
	public class RoomPropotype
	{
		public string Name;
		public string BuildingName;

		public RoomPropotype(string name, string buildingName)
		{
			Name = name;
			BuildingName = buildingName;
		}
	}
}
