using System;
using System.Collections.Generic;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Klasa obsugujaca skanowanie
	/// </summary>
	public class Scanning
	{
		/// <summary>
		/// Numer id skanu
		/// </summary>
		private int Id = -1;
		
		/// <summary>
		/// Pokoj w ktorym wykonywany jest skan
		/// </summary>
		public Room Room { get; private set; }
		
		/// <summary>
		/// Lista zeskanowanych srodkow trwalych
		/// </summary>
		public List<ScanningPosition> Positions { get; private set; }

		/// <summary>
		/// Przygotowuje skanowanie
		/// </summary>
		/// <param name="room">Pokoj w ktorym wykonywany jest skan</param>
		/// <param name="InitialPositions">Lista zeskanowanych srodkow trwalych</param>
		/// <param name="id">Id skanu, jezeli zostanie pominiete, to skan nie zapisze sie w bazie</param>
		public Scanning(Room room, ScanningPosition[] InitialPositions, int id = -1)
		{
			Id = id;
			Room = room;
			Positions = new List<ScanningPosition>(InitialPositions);
		}

		public void AddNewPosition()
		{
			throw new System.Exception("Not implemented");
		}

		public ScanPrototype GeneratePrototype()
		{
			throw new System.Exception("Not implemented");
		}

		public bool SendToDB()
		{
			throw new System.Exception("Not implemented");
		}

		public void DeletePosition(int index)
		{
			throw new System.Exception("Not implemented");
		}

		public void MoveAllAssetsToThisRoom()
		{
			throw new System.Exception("Not implemented");
		}

		public ReportPrototype GenerateRaport()
		{
			throw new System.Exception("Not implemented");
		}
	}
}
