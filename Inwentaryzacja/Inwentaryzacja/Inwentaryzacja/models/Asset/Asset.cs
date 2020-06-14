using System;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Klasa przetrzymujaca dane srodka trwalego
	/// </summary>
	public class Asset 
	{
		/// <summary>
		/// Unikatowy numer id srodka trwalego
		/// </summary>
		public int Id { get; private set; }
		
		/// <summary>
		/// Typ srodka trwalego
		/// </summary>
		public AssetType Type { get; private set; }

		/// <summary>
		/// Konstruktor srodka trwalego
		/// </summary>
		/// <param name="id">Numer id srodka trwalego</param>
		/// <param name="type">Typ srodka trwalego</param>
		public Asset(int id, AssetType type)
		{
			Id = id;
			Type = type;
		}
	}
}
