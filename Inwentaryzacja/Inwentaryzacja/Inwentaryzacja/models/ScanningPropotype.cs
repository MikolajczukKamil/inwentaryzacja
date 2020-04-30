using System;

namespace Inwentaryzacja.models
{
	public class ScanningPropotype
	{
		public int ScanningId;
		public ScanningPosition[] Positions;

		public ScanningPropotype(int id, ScanningPosition[] positions)
		{
			ScanningId = id;
			Positions = positions;
		}
	}
}
