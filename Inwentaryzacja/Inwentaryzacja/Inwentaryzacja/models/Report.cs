using System;

namespace Inwentaryzacja.models
{
	public class Report 
	{
		private ReportPosition[] _reportPostions;
		private Room _room;

		public int ReportId;
		public string Name;
		public int Room 
		public DateTime CreateDate;
		

		public Report(string name, int id, int room, DateTime date)
		{
			Name = name;
			ReportId = id;
			_room = room;
			CreateDate = date;
	
		}

		public bool ExportToPDF() {
			throw new System.Exception("Not implemented");
		}
		public bool Delete() {
			throw new System.Exception("Not implemented");
		}
	}
}