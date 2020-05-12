using System;
using System.Runtime.CompilerServices;

namespace Inwentaryzacja.Models
{
	public class AssetPrototype
	{
		public int type;

		public AssetPrototype(AssetType type)
		{
			this.type = type.Id;
		}
	}
}
