using Inwentaryzacja.Controllers.Api;
using System;
using System.Collections.Generic;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Prototyp raportu
	/// </summary>
	public class ReportPrototype
	{
		/// <summary>
		/// Nazwa raportu
		/// </summary>
		public string name;
		
		/// <summary>
		/// Id pokoju dla ktorego raport zostal wygenerowany
		/// </summary>
		public int room;
		
		/// <summary>
		/// Lista srodkow trwalych w raporcie
		/// </summary>
		public ReportPositionPrototype[] assets;

		/// <summary>
		/// Konstruktor prototypu raportu
		/// </summary>
		/// <param name="name">Nazwa raportu</param>
		/// <param name="room">Id pokoju dla ktorego raport zostal wygenerowany</param>
		/// <param name="postions">Lista srodkow trwalych w raporcie</param>
		public ReportPrototype(string name, RoomEntity room, ReportPositionPrototype[] postions)
		{
			this.name = name;
			this.room = room.id;
			this.assets = postions;
		}
	}
}
