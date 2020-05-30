using Inwentaryzacja.Controllers.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
			if (api.GetToken() != null)
			{
				return true;
			}
			else if (File.Exists(TokenPath))
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

				var task = Task.Run(() => api.getAssetInfo(1));

				if (task.Wait(5000))
				{
					if(task.Result != null)
					{
						return true;
					}
				}
			}

			return false;
		}
	}
}
