using Inwentaryzacja.Controllers.Api;
using System;
using System.Collections.Generic;

namespace Inwentaryzacja.Models
{
	public class ReportPrototype
	{
		public string name;
		public int room;
		public ReportPositionPrototype[] assets;

		public ReportPrototype(string name, RoomEntity room, ReportPositionPrototype[] postions)
		{
			this.name = name;
			this.room = room.id;
			this.assets = postions;
		}
	}
}
