using System;
using System.Collections.Generic;

namespace Inwentaryzacja.models
{
	public class Scanning
	{
		private int _scanningId;
		private Room _room;
		private List<ScanningPosition> _positions;

		public int ScanningId => _scanningId;
		public Room Room => _room;
		public List<ScanningPosition> ScanningPositions => _positions;

		public Scanning(int id, Room room)
		{
			_scanningId = id;
			_room = room;
			_positions = new List<ScanningPosition>();
		}

		public void AddNewPosition()
		{
			throw new System.Exception("Not implemented");
		}

		public bool SendToDB()
		{
			throw new System.Exception("Not implemented");
		}

		public void DeletePosition()
		{
			throw new System.Exception("Not implemented");
		}

		public void MoveThingToThisRoom()
		{
			throw new System.Exception("Not implemented");
		}

		public ScanningReport GetScanningReport()
		{
			throw new System.Exception("Not implemented");
		}

		public ReportPrototype GenerateRaport()
		{
			throw new System.Exception("Not implemented");
		}
	}
}
