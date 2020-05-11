using System;
using System.Collections.Generic;

namespace Inwentaryzacja.Models
{
	public class ReportPrototype
	{
		public string Name;
		public Room Room;
		public ReportPosition[] Postions;

		public ReportPrototype(string name, Room room, ReportPosition[] postions)
		{
			Name = name;
			Room = room;
			Postions = postions; // todo: clone positions
		}
	}
}
