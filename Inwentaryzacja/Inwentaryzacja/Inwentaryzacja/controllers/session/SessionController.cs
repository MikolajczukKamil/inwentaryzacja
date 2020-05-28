using Inwentaryzacja.Controllers.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZXing.Aztec.Internal;

namespace Inwentaryzacja.controllers.session
{
    class SessionController
    {
		APIController api;
		private static string TokenFileName = "Token.txt";
		private static string TokenPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), TokenFileName);

		public SessionController(APIController api)
		{
			this.api = api;
		}

		public void SaveSessionToken()
        {
			StreamWriter sw = new StreamWriter(TokenPath,false);
			sw.WriteLine(api.GetToken());
			sw.Close();
		}

		public void RemoveSession()
		{
			if (File.Exists(TokenPath))
			{
				File.Delete(TokenPath);
			}

			api.DeleteToken();
		}

		public bool ResumeSession()
		{
			if (api.GetToken() != null)
			{
				return true;
			}
			else if (File.Exists(TokenPath))
			{
				StreamReader sr = new StreamReader(TokenPath);
				api.SetToken(sr.ReadLine());
				sr.Close();

				return true;
			}

			return false;
		}
	}
}
