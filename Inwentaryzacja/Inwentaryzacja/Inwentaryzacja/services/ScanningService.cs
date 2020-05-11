using System;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

namespace Inwentaryzacja.Services
{
	public class ScanningService
	{
		private APIController api;

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
