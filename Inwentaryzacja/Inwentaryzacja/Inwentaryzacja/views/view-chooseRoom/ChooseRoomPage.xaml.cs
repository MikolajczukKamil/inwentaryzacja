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
			GetBuildings();
		}

		private void BuildingPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			string choosenBuildingName = BuildingPicker.Items[BuildingPicker.SelectedIndex];
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

			if (buildingItem != null) GetRooms(buildingItem.id);
		}

		private async void GetRooms(int buildingId)
		{
			Task<RoomEntity[]> getRoomsTask = api.getRooms(buildingId);
			await getRoomsTask;
			rooms = getRoomsTask.Result;

			foreach (RoomEntity item in rooms)
			{
				RoomPicker.Items.Add(item.name);
			}
		}

		private async void GetBuildings()
		{
			Task<BuildingEntity[]> getBuildingsTask = api.getBuildings();
			await getBuildingsTask;
			buildings = getBuildingsTask.Result;

			//Problem w tym, że wyświetla się alert niżej, jakbym dostał nulla z metody api.GetBuildings()
			if (buildings == null) await DisplayAlert("Zmiennna", "Buildings jest nullem", "Wyjdz");

			//I przez to aplikacja wywala się gdy dochodzi do wykonania tego kodu
			foreach (BuildingEntity item in buildings)
			{
				BuildingPicker.Items.Add(item.name);
			}
		}
	}
}