using System;
using System.Collections.Generic;

namespace Inwentaryzacja.models
{
	public class Scanning
	{
		private int _Id = -1;
		public Room Room { get; private set; }
		public List<ScanningPosition> Positions { get; private set; }

		/// <param name="id">Unused, if scanning is not save in database</param>
		public Scanning(Room room, ScanningPosition[] InitialPositions, int id = -1)
		{
			_Id = id;
			Room = room;
			Positions = new List<ScanningPosition>(InitialPositions);
		}

		public void AddNewPosition()
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
