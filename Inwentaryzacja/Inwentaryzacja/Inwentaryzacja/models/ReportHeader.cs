using System;

namespace Inwentaryzacja.models
{ 
	public class ReportHeader
	{
		public int ReportId;
		public string ReportName;
		public string RoomName;
		public DateTime CreateDate;

		public ReportHeader(int id, string name, string roomName, DateTime date)
		{
			ReportId = id;
			RoomName = roomName;
			ReportName = name;
			CreateDate = date;
		}
	}
}
