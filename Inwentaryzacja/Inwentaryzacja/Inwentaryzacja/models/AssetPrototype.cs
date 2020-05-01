using System;
using System.Runtime.CompilerServices;

namespace Inwentaryzacja.models
{
	public class AssetPrototype
	{
		public int AssetId;
		public AssetType AsseType;

		public AssetPrototype(int id, AssetType type)
		{
			AssetId = id;
			AsseType = type;
		}
	}
}