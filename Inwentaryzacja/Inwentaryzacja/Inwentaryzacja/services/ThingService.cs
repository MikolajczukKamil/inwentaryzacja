using System;
using System.Threading.Tasks;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

public class ThingService 
{
	private APIController ApiController;

	public ThingService(APIController apiController)
	{
		ApiController = apiController;
	}

	public Asset GetAsset(int id) {

		AssetEntity assetEntity = ApiController.getAssetByID(id).Result;
		Asset asset = new Asset(assetEntity.name, assetEntity.id, assetEntity.assetType);

		return asset;
				
	}
	public bool AddAsset(AssetPrototype newAsset) 
	{
		bool sent = ApiController.createAsset(newAsset).Result;

		return sent;
	}
	public bool DeleteAsset(int id) 
	{
		bool deleted = ApiController.deleteAssetByID(id).Result;

		return deleted;
	}

}
