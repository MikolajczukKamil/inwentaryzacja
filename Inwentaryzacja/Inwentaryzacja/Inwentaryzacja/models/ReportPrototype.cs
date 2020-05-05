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
				this.assets.Add(new ReportAssetsPrototype(item.AssetId));
			}
		}

		public class ReportAssetsPrototype
		{
			public int asset_id { get; set; }

			public ReportAssetsPrototype(int asset_id)
			{
				this.asset_id = asset_id;
			}
		}
	}
}