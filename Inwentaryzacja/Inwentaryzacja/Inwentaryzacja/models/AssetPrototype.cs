using System;
using System.Runtime.CompilerServices;

namespace Inwentaryzacja.models
{
	public class AssetPrototype
	{
		public int type { get; set; }

		public AssetPrototype(int type)
		{
			this.type = type;
		}
	}
}