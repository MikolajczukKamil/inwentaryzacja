using System;
using System.Runtime.CompilerServices;

namespace Inwentaryzacja.models
{
	public class AssetPrototype
	{
		public string name;
		public AssetType type;

		public AssetPrototype(string name, AssetType type)
		{
			this.name = name;
			this.type = type;
		}
	}
}
