using System;

namespace Inwentaryzacja.models
{
	public class Asset 
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public AssetType Type { get; private set; }

		public Asset(int id, string name, AssetType type)
		{
			Id = id;
			Name = name;
			Type = type;
		}
	}
}
