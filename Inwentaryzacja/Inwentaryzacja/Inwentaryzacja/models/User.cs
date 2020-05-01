using System;
using System.Data;
using Inwentaryzacja.controllers;

namespace Inwentaryzacja.models
{
	public class User
	{
		private APIController _aPIController;
		private string _login;
		private string _token;
		public string Login => _login;
		public string Token => _token;

		public User(string login, string token)
		{
			_login = login;
			_token = token;
		}

		public User _LogIn()
		{
			throw new System.Exception("Not implemented");
		}

	}
}
