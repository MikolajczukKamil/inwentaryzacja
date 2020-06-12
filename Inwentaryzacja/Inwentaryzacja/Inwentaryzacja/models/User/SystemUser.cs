using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Zalogowany uzytkownik
	/// </summary>
    class SystemUser : User
	{
		/// <summary>
		/// Token do autentykacji
		/// </summary>
		public string Token { get; private set; }

		/// <summary>
		/// Konstruktor zalogowanego uzytkownika
		/// </summary>
		/// <param name="id">Id uzytkownika</param>
		/// <param name="login">Login uzytkownika</param>
		/// <param name="token">Token do autentykacji</param>
		public SystemUser(int id, string login, string token): base(id, login)
		{
			Token = token;
		}
	}
}
