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
		public int BuildingName;

		public Room(string name, int buildingName, int id)
		{
			Name = name;
			BuildingName = buildingName;
			RoomId = id;}
	}
}
