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
	    /// <summary>
	    /// Obiekt do interakcji z API
	    /// </summary>
		APIController api;
	    
	    /// <summary>
	    /// Nazwa pliku z tokenem
	    /// </summary>
		private static string TokenFileName = "Token.txt";
	    
	    /// <summary>
	    /// Ścieżka do pliku z tokenem
	    /// </summary>
		private static string TokenPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), TokenFileName);

	    /// <summary>
	    /// Przypisuje podany obiekt kontrolera API
	    /// </summary>
	    /// <param name="api">Obiekt kontrolera API</param>
		public SessionController(APIController api)
		{
			this.api = api;
		}

	    /// <summary>
	    /// Zapisuje do pliku token obecnej sesji
	    /// </summary>
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

	    /// <summary>
	    /// Usuwa plik z tokenem
	    /// </summary>
		public void RemoveSession()
		{
			if (File.Exists(TokenPath))
			{
				File.Delete(TokenPath);
			}

			api.DeleteToken();
		}

	    /// <summary>
	    /// Sprawdza czy użytkownik jest zalogowany (czy istnieje zapisany token)
	    /// Jeżeli istnieje zapisany token, ustawia ten token w API
	    /// </summary>
	    /// <returns>Czy użytkownik jest zalogowany</returns>
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
