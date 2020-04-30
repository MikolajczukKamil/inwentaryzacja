using System;
using Inwentaryzacja.models;

namespace Inwentaryzacja.models
{ 
	public class AssetType
	{
		private AssetPrototype _assetPrototype;
		private Asset _asset;

		public char Letter;
		public string Name;

		public AssetType(string name, char letter, AssetPrototype prototype, Asset asset)
		{
			Name = name;
			Letter = letter;
			_assetPrototype = prototype;
			_asset = asset;
		}
	}
}
