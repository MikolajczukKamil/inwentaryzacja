using System;

namespace Inwentaryzacja.models
{
	public class Asset 
	{
		private AssetType _assetType;
		private ReportPosition _reportPostion;
		private ScanningPosition _scanningPosition;

		public int AssetId;
		public AssetType AssetType => _assetType;
		public string Name;

		public Asset(string name, int id, AssetType type, ReportPosition reportPosition, ScanningPosition scanningPosition)
		{
			Name = name;
			AssetId = id;
			_assetType = type;
			_reportPostion = reportPosition;
			_scanningPosition = scanningPosition;
		}

		public bool Delete() 
		{
			throw new System.Exception("Not implemented");
		}
	}
}
