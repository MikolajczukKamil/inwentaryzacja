using System;
using System.Collections.Generic;

namespace Inwentaryzacja.models
{
	public class ReportPrototype
	{
		public string name { get; set; }
		public int room { get; set; }
		public List<ReportAssetsPrototype> assets;

		public ReportPrototype(string name, int room, List<Asset> assets)
		{
			this.name = name;
			this.room = room;
			this.assets = new List<ReportAssetsPrototype>();
			foreach (var item in assets)
			{
				this.assets.Add(new ReportAssetsPrototype(item.AssetId, null));
			}
		}

		public class ReportAssetsPrototype
		{
			public int id { get; set; }
			public bool? present { get; set; }

			public ReportAssetsPrototype(int id, bool? present)
			{
				this.id = id;
				this.present = present;
			}
		}
	}
}