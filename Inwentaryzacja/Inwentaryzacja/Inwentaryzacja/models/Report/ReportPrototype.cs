using System;
using System.Collections.Generic;

namespace Inwentaryzacja.Models
{
	public class ReportPrototype
	{
		public string name;
		public int room;
		public ReportPositionPrototype[] assets;

		public ReportPrototype(string name, ReportPositionPrototype[] postions)
		{
			this.name = name;
			this.assets = postions;
		}
	}
}