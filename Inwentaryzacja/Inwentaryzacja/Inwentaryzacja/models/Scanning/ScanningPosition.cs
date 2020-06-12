using System;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Srodek trwaly w skanie
	/// </summary>
	public class ScanningPosition
	{
		/// <summary>
		/// Srodek trwaly
		/// </summary>
        public Asset Asset { get; private set; }
		
		/// <summary>
		/// Czy srodek trwaly powinien znajdowac sie w tym pokoju
		/// </summary>
        public bool Present { get; private set; }
		
		/// <summary>
		/// Pokoj w ktorym poprzednio znajdowal sie srodek trwaly
		/// </summary>
        public Room Previus { get; private set; }

		/// <summary>
		/// Konstruktor srodka trwalego w skanie
		/// </summary>
		/// <param name="asset">Srodek trwaly</param>
		/// <param name="previus">Poprzedni pokoj srodka trwalego</param>
		/// <param name="present">Czy srodek trwaly powinien znajdowac sie w tym pokoju</param>
        public ScanningPosition(Asset asset, Room previus, bool present)
		{
            Asset = asset;
            Present = present;
            Previus = previus;
        }
		
		public ScanPositionPropotype GeneratePrototype()
        {
            throw new System.Exception("Not implemented");
        }
	}
}

