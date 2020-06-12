using System;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Klasa przetrzymujaca dane raportu
	/// </summary>
	public class Report 
	{
		/// <summary>
		/// Unikatowy numer id raportu
		/// </summary>
		public int Id { get; private set; }
		
		/// <summary>
		/// Nazwa raportu
		/// </summary>
		public string Name { get; private set; }
		
		/// <summary>
		/// Pokoj dla ktorego raport zostal wygenerowany
		/// </summary>
		public Room Room { get; private set; }
		
		/// <summary>
		/// Data wygenerowania raportu
		/// </summary>
		public DateTime CreateDate { get; private set; }
		
		/// <summary>
		/// Lista srodkow trwalych w raporcie
		/// </summary>
		public ReportPosition[] Postions { get; private set; }

		/// <summary>
		/// Konstruktor raportu
		/// </summary>
		/// <param name="id">Numer id raportu</param>
		/// <param name="name">Nazwa raportu</param>
		/// <param name="room">Pokoj dla ktorego raport zostal wygenerowany</param>
		/// <param name="date">Data wygenerowania raportu</param>
		/// <param name="postions">Lista srodkow trwalych w raporcie</param>
		public Report(int id, string name, Room room, DateTime date, ReportPosition[] postions)
		{
			Id = id;
			Name = name;
			Room = room;
			Postions = postions;
			CreateDate = date;
		}

		/// <summary>
		/// Konstruktor raportu
		/// </summary>
		/// <param name="header">Metadane raportu</param>
		/// <param name="postions">Lista srodkow trwalych w raporcie</param>
		public Report(ReportHeader header, ReportPosition[] postions)
		{
			Id = header.Id;
			Name = header.Name;
			Room = header.Room;
			Postions = postions;
			CreateDate = header.CreateDate;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public bool ExportToPDF() {
			throw new System.Exception("Not implemented");
		}
	}
}
