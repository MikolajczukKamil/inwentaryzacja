using System;
using System.Threading.Tasks;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;

namespace Inwentaryzacja.Services
{
	/// <summary>
	/// Klasa odpowiadajaca za obsluge srodkow trwalych
	/// </summary>
	public class AssetService
	{
		private APIController api;

		/// <summary>
		/// Konstruktor klasy
		/// </summary>
		/// <param name="apiController">obiekt do interakcji z api </param>
		public AssetService(APIController apiController)
		{
			api = apiController;
		}

		
		public AssetInfo GetAssetInfo(Asset asset)
		{

			// AssetInfoEntity assetInfoEntity = api.getAssetInfo(asset.Id).Result; // To AssetInfo

			throw new System.Exception("Not implemented");
		}
	}
}
