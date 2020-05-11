using System;

namespace Inwentaryzacja.models
{ 
	public class ReportHeader
	{
		public int ReportId;
		public string ReportName;

		public ReportHeader(int id, string name)
		{
			ReportId = id;
			ReportName = name;
		}
	}
}
