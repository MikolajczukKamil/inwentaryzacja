using System;

namespace Inwentaryzacja.Models
{
	public class Report 
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public Room Room { get; private set; }
		public DateTime CreateDate { get; private set; }
		public ReportPosition[] Postions { get; private set; }


		public Report(int id, string name, Room room, DateTime date, ReportPosition[] postions)
		{
			Id = id;
			Name = name;
			Room = room;
			Postions = postions;
			CreateDate = date;
		}

		public Report(ReportHeader header, ReportPosition[] postions)
		{
			Id = header.Id;
			Name = header.Name;
			//Room = header.Room;
			Postions = postions;
			CreateDate = header.CreateDate;
		}

		public bool ExportToPDF() {
			throw new System.Exception("Not implemented");
		}
	}
}
