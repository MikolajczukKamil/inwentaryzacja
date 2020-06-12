using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Klasa przetrzymujaca dane srodku trwalym w sali
	/// </summary>
    public class AssetInfo
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
	    /// Pokoj w ktorym znajduje sie srodek trwaly
	    /// </summary>
		public Room Room { get; private set; }

	    /// <summary>
	    /// Konstruktor srodka trwalego
	    /// </summary>
	    /// <param name="id">Numer id srodka trwalego</param>
	    /// <param name="type">Typ srodka trwalego</param>
	    /// <param name="room">Pokoj w ktorym znajduje sie srodek trwaly</param>
		public AssetInfo(int id, AssetType type, Room room)
		{
			Id = id;
			Type = type;
			Room = room;
		}
	}
}
