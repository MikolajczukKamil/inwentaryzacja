using System;
using System.Threading.Tasks;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

public class ThingService 
{
	private APIController ApiController;

	public ThingService(APIController apiController)
	{
		ApiController = new APIController();
	}

	public Asset GetAsset(int id) {

		AssetEntity assetEntity = ApiController.getAssetByID(id).Result;
		Asset asset = new Asset(assetEntity.name, assetEntity.id, assetEntity.asset_type);

		return asset;
				
	}
	public bool AddAsset(Asset newAsset) 
	{
		bool sent = ApiController.sendAsset(newAsset.Name, newAsset.AssetId).Result;

		return sent;
	}
	public bool DeleteAsset(int id) 
	{
		bool deleted = ApiController.deleteAssetByID(id).Result;

		return deleted;
	}

}
