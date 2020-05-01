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

		public Room(string name, string buildingName, int id, RoomService roomService, Report report, 
			ReportPosition reportPosition, ReportPrototype reportPrototype, Scanning scanning, ScanningPosition scanningPosition)
		{
			Name = name;
			BuildingName = buildingName;
			RoomId = id;
			_roomService = roomService;
			_report = report;
			_reportPosition = reportPosition;
			_reportPrototype = reportPrototype;
			_scanning = scanning;
			_scanningPosition = scanningPosition;

		}
	}
}
