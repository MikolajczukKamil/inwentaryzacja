using System;
using System.Collections.Generic;

namespace Inwentaryzacja.Models
{
	public class ReportPrototype
	{
		public int room;
		public ReportPositionPrototype[] assets;
		public string name;

		public ReportPrototype(string name, Room room, ReportPositionPrototype[] postions)
		{
			this.name = name;
			this.room = room.Id;
			this.assets = postions;
		}
	}
}
