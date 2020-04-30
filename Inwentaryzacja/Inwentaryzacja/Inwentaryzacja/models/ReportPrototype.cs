using System;

namespace Inwentaryzacja.models
{
	public class ReportPrototype
	{
		public Scanning Scanning;
		public string Name;
		public Room Room;
		public ReportPosition[] Positions;

		public ReportPrototype(string name, Room room, ReportPosition[] position, Scanning scanning)
		{
			Name = name;
			Room = room;
			Positions = position;
			Scanning = scanning;
		}
	}
}