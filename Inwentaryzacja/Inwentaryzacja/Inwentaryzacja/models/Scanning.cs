using System;

namespace Inwentaryzacja.models
{
	public class Scanning
	{
		private int _scanningId;
		private Room _room;
		private ScanningPosition[] _positions;

		public int ScanningId => _scanningId;
		public Room Room => _room;
		public ScanningPosition[] ScanningPositions => _positions;

		public Scanning(int id, Room room, ScanningPosition[] positions)
		{
			_scanningId = id;
			_room = room;
			_positions = positions;
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
