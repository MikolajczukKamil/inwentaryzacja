using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace Inwentaryzacja
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChooseRoomPage : ContentPage
	{
		List<string> _pickerItems = new List<string>();
		public List<string> PickerItems => _pickerItems;


		//Method to be edited that is loading list of string for picker
		private void LoadPickerItems()
		{
			_pickerItems.Add("Testowa Sala");
			_pickerItems.Add("3/38");
			_pickerItems.Add("Aula 1");
		}

		//Method which initialized List of String for Picker - must be put after InitializeComponent()
		private void InitializePicker()
		{
			foreach (string itemName in _pickerItems)
			{
				RoomPicker.Items.Add(itemName);
			}

		}

		public ChooseRoomPage ()
		{
			LoadPickerItems();
			InitializeComponent();
			InitializePicker();
		}

		private async void _logOutButton_Clicked(object sender, EventArgs e)
		{
			string TokenFileName = "Token.txt";    //Nazwa pliku gdzie zapisuje token                             
			string WhereTokenIs = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Sciezka gdzie jest plik Token.txt	
			string DeleteTokenFromPath = Path.Combine(WhereTokenIs, TokenFileName); // Sciezka + Nazwa pliku


			if (!File.Exists(DeleteTokenFromPath))
			{
				try
				{
					// Jesli istnieje usuwam
					File.Delete(DeleteTokenFromPath); // Usuwam plik Token.txt          
					if (Navigation.NavigationStack.Count == 0)
					{                                 // Redirect strona logowania
						App.Current.MainPage = new NavigationPage(new LoginPage());
					}
					else
					{
						await Navigation.PopAsync();
					}
				}                                   // Jesli plik nie istnieje komunikat 
				catch { await DisplayAlert("Uzytkownik nie jest zalagowany", "Zalogowac sie?", "ok"); }
			}
		}

	}
}