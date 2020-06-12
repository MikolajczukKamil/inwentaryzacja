using System;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Klasa przetrzymujaca dane pokoju
	/// </summary>
	public class Room
	{
		/// <summary>
		/// Unikatowy numer id pokoju
		/// </summary>
		public int Id { get; private set; }
		
		/// <summary>
		/// Nazwa pokoju
		/// </summary>
		public string Name { get; private set; }
		
		/// <summary>
		/// Budynek w ktorym znajduje sie pokoj
		/// </summary>
		public Building Building { get; private set; }

		/// <summary>
		/// Konstruktor pokoju
		/// </summary>
		/// <param name="id">Numer id pokoju</param>
		/// <param name="name">Nazwa pokoju</param>
		/// <param name="building">Budynek w ktorym znajduje sie pokoj</param>
		public Room(int id, string name, Building building)
		{
			Id = id;
			Name = name;
			Building = building;
		}

		/// <summary>
		/// Sprawdza czy podany pokoj jest tym samym pokojem
		/// </summary>
		/// <param name="other">Pokoj do porowania</param>
		/// <returns>Czy podany pokoj jest tym samym pokojem</returns>
		public bool SameAs(Room other)
		{
			return other == null || Id == other.Id;
		}
	}
}
