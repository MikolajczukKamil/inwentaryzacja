using System;

namespace Inwentaryzacja.models
{
	public class Asset 
	{
		public int Id { get; private set; }
		public AssetType Type { get; private set; }

		public Asset(int id, AssetType type)
		{
			Id = id;
			Type = type;
		}
	}
}
