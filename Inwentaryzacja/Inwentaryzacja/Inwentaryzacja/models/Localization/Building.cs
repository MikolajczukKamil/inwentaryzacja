using System;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Klasa przetrzymujaca dane budynku
	/// </summary>
	public class Building
	{
		/// <summary>
		/// Unikatowy numer id budynku
		/// </summary>
		public int Id { get; private set; }
		
		/// <summary>
		/// Nazwa budynku
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Konstruktor budynku
		/// </summary>
		/// <param name="id">Numer id budynku</param>
		/// <param name="name">Nazwa budynku</param>
		public Building(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
