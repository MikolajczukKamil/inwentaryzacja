using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.Models
{
    class SystemUser : User
	{
		public string Token { get; private set; }

		public SystemUser(int id, string login, string token): base(id, login)
		{
			Token = token;
		}
	}
}
