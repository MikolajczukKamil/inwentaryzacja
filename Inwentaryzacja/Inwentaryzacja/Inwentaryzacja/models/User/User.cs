using System;
using System.Data;
using Inwentaryzacja.controllers;

namespace Inwentaryzacja.models
{
	public class User
	{
		public string Login { get; private set; }
		public string Token { get; private set; }

		public User(string login, string token)
		{
			Login = login;
			Token = token;
		}

		public bool _LogIn()
		{
			return true;
		}
	}
}
