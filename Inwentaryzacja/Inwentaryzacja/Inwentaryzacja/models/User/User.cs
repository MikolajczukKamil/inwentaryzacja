using System;
using System.Data;
using Inwentaryzacja.Controllers;

namespace Inwentaryzacja.Models
{
	public class User
	{
		public int Id { get; private set; }
		public string Login { get; private set; }

		public User(int id, string login)
		{
			Id = id;
			Login = login;
		}
	}
}
