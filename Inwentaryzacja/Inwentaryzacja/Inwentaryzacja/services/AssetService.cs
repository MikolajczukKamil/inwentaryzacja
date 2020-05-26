using System;
using System.Threading.Tasks;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;

namespace Inwentaryzacja.Services
{
	public class AssetService
	{
		private APIController api;

		public AssetService(APIController apiController)
		{
			api = apiController;
		}

		public AssetInfo GetAssetInfo(Asset asset)
		{

			// AssetInfoEntity assetInfoEntity = api.getAssetInfo(asset.Id).Result; // To AssetInfo

			throw new System.Exception("Not implemented");
		}

		public bool AddAsset(AssetPrototype newAsset)
		{
			return api.CreateAsset(newAsset).Result;
		}
	}
}