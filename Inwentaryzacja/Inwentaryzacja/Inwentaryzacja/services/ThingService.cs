using System;
using System.Threading.Tasks;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

public class ThingService {

	private APIController api = new APIController();
	
	public Asset GetAsset(int id) {

		AssetEntity assetEntity = api.getAssetByID(id).Result;
		Asset asset = new Asset(assetEntity.name, assetEntity.id, assetEntity.asset_type);

		return asset;
				
	}
	public bool AddAsset(Asset newAsset) 
	{
		bool sent = api.sendAsset(newAsset.name, newAsset.assetId).Result;

		return sent;
	}
	public bool DeleteAsset(int id) 
	{
		bool deleted = api.deleteAssetByID(id).Result;

		return deleted;
	}

}
