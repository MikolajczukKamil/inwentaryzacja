using System;

namespace Inwentaryzacja.Models
{ 
	/// <summary>
	/// Metadane raportu
	/// </summary>
	public class ReportHeader
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
		/// Wlasciciel raportu
		/// </summary>
		public User Owner { get; private set; }
		
		/// <summary>
		/// Data wygenerowania raportu
		/// </summary>
		public DateTime CreateDate { get; private set; }

		/// <summary>
		/// Konstruktor metadanych raportu
		/// </summary>
		/// <param name="id">Numer id raportu</param>
		/// <param name="name">Nazwa raportu</param>
		/// <param name="room">Pokoj dla ktorego raport zostal wygenerowany</param>
		/// <param name="owner">Wlasciciel raportu</param>
		/// <param name="date">Data wygenerowania raportu</param>
		public ReportHeader(int id, string name, Room room, User owner, DateTime date)
		{
			Id = id;
			Name = name;
			Room = room;
			Owner = owner;
			CreateDate = date;
		}
	}
}
