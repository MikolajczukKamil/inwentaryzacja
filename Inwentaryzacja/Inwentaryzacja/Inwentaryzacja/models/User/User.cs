using System;
using System.Data;
using Inwentaryzacja.Controllers;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Klasa przetrzymujaca dane uzytkownika
	/// </summary>
	public class User
	{
		/// <summary>
		/// Unikatowy numer id uzytkownika
		/// </summary>
		public int Id { get; private set; }
		
		/// <summary>
		/// Login uzytkownika
		/// </summary>
		public string Login { get; private set; }

		/// <summary>
		/// Konstruktor uzytkownika
		/// </summary>
		/// <param name="id">Numer id uzytkownika</param>
		/// <param name="login">Login uzytkownika</param>
		public User(int id, string login)
		{
			Id = id;
			Login = login;
		}
	}
}
