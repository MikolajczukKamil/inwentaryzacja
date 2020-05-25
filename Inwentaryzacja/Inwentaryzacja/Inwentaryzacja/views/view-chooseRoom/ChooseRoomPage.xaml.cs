using Inwentaryzacja.views.view_chooseRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
		public void AddRoom_clicked(object o, EventArgs args)
		{
			App.Current.MainPage = new NavigationPage(new AddRoom());
		}
	}
}