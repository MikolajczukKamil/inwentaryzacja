using System;
using System.Runtime.CompilerServices;

namespace Inwentaryzacja.models
{
	public class AssetPrototype
	{
		public string name { get; set; }
		public int asset_type { get; set; }

		public AssetPrototype(string name, int asset_type)
		{
			this.name = name;
			this.asset_type = asset_type;
		}
	}
}