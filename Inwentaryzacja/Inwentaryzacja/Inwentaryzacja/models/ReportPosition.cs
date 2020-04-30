using System;

namespace Inwentaryzacja.models
{
	public class ReportPosition
	{
		private Room _room;
		private Asset _asset;
		private Report _report;
		private ReportPrototype _reportPrototype;

		public Asset Asset => _asset;
		public Room PreviousRoom => _room;

		public ReportPosition(Room pRoom, Asset asset, Report report, ReportPrototype reportPrototype)
		{
			_room = pRoom;
			_asset = asset;
			_report = report;
			_reportPrototype = reportPrototype;
		}
	}
}
