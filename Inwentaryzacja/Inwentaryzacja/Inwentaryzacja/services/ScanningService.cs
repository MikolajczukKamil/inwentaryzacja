using System;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;

namespace Inwentaryzacja.Services
{
	/// <summary>
	/// Klasa odpowiadajaca za obsluge skanowania
	/// </summary>
	public class ScanningService
	{
		private APIController api;

		/// <summary>
		/// Konstruktor klasy
		/// </summary>
		/// <param name="apiController">obiekt do interakcji z api</param>
		public ScanningService(APIController apiController)
		{
			api = apiController;
		}


		public Scanning CreateScaning()
		{
			throw new System.Exception("Not implemented");
		}

		/// <summary>
		/// Unused
		/// </summary>
		/// <param name="scanning"></param>
		public void UpdateScanning(Scanning scanning)
		{
			throw new System.Exception("Not implemented");
		}
	} 
}
