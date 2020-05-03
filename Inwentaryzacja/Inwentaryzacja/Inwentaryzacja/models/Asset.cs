using System;

namespace Inwentaryzacja.models
{
	public class Asset 
	{
		private AssetType _assetType;
		private ReportPosition _reportPostion;
		private ScanningPosition _scanningPosition;

		public AssetType AssetType => _assetType;
		public int assetId;
		public string name;
		public int assetType;

		public Asset(string name, int id, AssetType type, ReportPosition reportPosition, ScanningPosition scanningPosition)
		{
			name = name;
			assetId = id;
			_assetType = type;
			_reportPostion = reportPosition;
			_scanningPosition = scanningPosition;
		}

		public Asset(string name, int assetId, int assetType)
		{
			this.assetId = assetId;
			this.name = name;
			this.assetType = assetType;
		}

		public bool Delete() 
		{
			throw new System.Exception("Not implemented");
		}
	}
}
