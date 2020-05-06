using System;

namespace Inwentaryzacja.models
{
	public class Room
	{
		private RoomService _roomService;
		private Report _report;
		private ReportPosition _reportPosition;
		private ReportPrototype _reportPrototype;
		private Scanning _scanning;
		private ScanningPosition _scanningPosition;

		public int RoomId;
		public string Name;
		public string BuildingName;
		public int BuildingId;

		public Room(string name, int buildingId, int id)
		{
			Name = name;
			BuildingId = buildingId;
			RoomId = id;
	  }
}
