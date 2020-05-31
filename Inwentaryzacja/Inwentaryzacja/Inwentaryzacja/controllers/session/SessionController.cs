using Inwentaryzacja.Controllers.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
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

			try
			{
				sw.WriteLine(api.GetToken());
			}
			catch (Exception)
			{
				sw.Close();
			}
			
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

		public bool IsLogin()
		{
			if (File.Exists(TokenPath))
			{
				StreamReader sr = new StreamReader(TokenPath);

				try
				{
					api.SetToken(sr.ReadLine());
				}
				catch (Exception)
				{
					sr.Close();
					return false;
				}
				
				sr.Close();

				return true;
			}

			return false;
		}
	}
}
