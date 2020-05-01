using System;

namespace Inwentaryzacja.models
{
	public class Report 
	{
		private ReportPosition[] _reportPostions;
		private Room _room;

		public int ReportId;
		public string Name;
		public Room Room => _room;
		public DateTime CreateDate;
		public ReportPosition[] Positions => _reportPostions;

		public Report(string name, int id, Room room, DateTime date, ReportPosition[] reportPositions)
		{
			Name = name;
			ReportId = id;
			_room = room;
			CreateDate = date;
			_reportPostions = reportPositions;
		}

		public bool ExportToPDF() {
			throw new System.Exception("Not implemented");
		}
		public bool Delete() {
			throw new System.Exception("Not implemented");
		}
	}
}