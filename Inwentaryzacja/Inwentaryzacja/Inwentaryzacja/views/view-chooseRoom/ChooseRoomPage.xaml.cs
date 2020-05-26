using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Inwentaryzacja.views.view_chooseRoom;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChooseRoomPage : ContentPage
	{
		RoomEntity[] rooms;
		BuildingEntity[] buildings;

		APIController api = new APIController();
		
		public ChooseRoomPage ()
		{
			InitializeComponent();

			api.ErrorEventHandler += onApiError;

			GetBuildings();
		}

		private void BuildingPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			string choosenBuildingName = BuildingPicker.Items[BuildingPicker.SelectedIndex];

			RoomPicker.Items.Clear();

			GetBuildingRooms(choosenBuildingName);
		}

		public void AddBuildingClicked(object o, EventArgs e)
		{
			PopupNavigation.Instance.PushAsync(new AddBuildingView());
		}

		private void GetBuildingRooms(string name)
		{
			BuildingEntity buildingItem = null;

			foreach (BuildingEntity item in buildings)
			{
				if (name == item.name)
				{
					buildingItem = item;
					break;
				}
			}

			if (buildingItem != null)
			{
				GetRooms(buildingItem.id);
			}
		}

		private async void GetRooms(int buildingId)
		{
			rooms = await api.getRooms(buildingId);

			foreach (RoomEntity item in rooms)
			{
				RoomPicker.Items.Add(item.name);
			}
		}

		private async void GetBuildings()
		{
			buildings = await api.getBuildings();

			if (buildings == null)
			{
				return;
			}

			foreach (BuildingEntity item in buildings)
			{
				BuildingPicker.Items.Add(item.name);
			}
		}

		private async void onApiError(object o, ErrorEventArgs error)
		{
			await DisplayAlert("Błąd", error.MessageForUser, "Wyjdz");
		}
	}
}