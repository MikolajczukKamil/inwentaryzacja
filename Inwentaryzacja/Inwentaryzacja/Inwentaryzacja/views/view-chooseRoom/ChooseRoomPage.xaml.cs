using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Inwentaryzacja.views.view_chooseRoom;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChooseRoomPage : ContentPage
	{
		RoomEntity[] rooms;
		BuildingEntity[] buildings;
		bool addedNewBuilding = false;

		APIController api = new APIController();
		public ChooseRoomPage()
		{
			InitializeComponent();
			api.ErrorEventHandler += onApiError;
			BindingContext = this;
			GetBuildings();
		}
		public ChooseRoomPage(bool addedNewBuilding)
		{
			this.addedNewBuilding = addedNewBuilding;
			InitializeComponent();
			BindingContext = this;			
			api.ErrorEventHandler += onApiError;
			GetBuildings();
		}

		private void BuildingPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			string choosenBuildingName = BuildingPicker.Items[BuildingPicker.SelectedIndex];
			GetBuildingRooms(choosenBuildingName);
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
			rooms = await api.getRooms(buildingId);

			Task<RoomEntity[]> roomsTask = api.getRooms(buildingId);
			EnableView(false);
			await roomsTask;
			EnableView(true);
			rooms = roomsTask.Result;
			

			if (rooms == null)
			{
				return;
			} 

			if (RoomPicker.Items.Count > 0) RoomPicker.Items.Clear();

			foreach (RoomEntity item in rooms)
			{
				RoomPicker.Items.Add(item.name);
			}

			if (RoomPicker.Items.Count > 0) RoomPicker.IsEnabled = true;
			else RoomPicker.IsEnabled = false;
		}		
		
		private async void GetBuildings()
		{
			Task<BuildingEntity[]> buildingTask = api.getBuildings();
			EnableView(false);
			await buildingTask;
			EnableView(true);
			buildings = buildingTask.Result;

			if (buildings == null) return;

			foreach (BuildingEntity item in buildings)
			{
				BuildingPicker.Items.Add(item.name);
			}

			if (BuildingPicker.Items.Count > 0)
			{
				if (addedNewBuilding)
				{
					BuildingPicker.SelectedItem = BuildingPicker.Items[BuildingPicker.Items.Count - 1];
				}
				else
				{
					BuildingPicker.SelectedItem = BuildingPicker.Items[0];
				}
				
			}
		}

		private void EnableView(bool state)
		{
			IsBusy = !state;
			RoomPicker.IsEnabled = state;
			BuildingPicker.IsEnabled = state;
			BackBtn.IsEnabled = state;
			AddBuildingBtn.IsEnabled = state;
			AddRoomBtn.IsEnabled = state;			
		}

		private async void Continue_Button_Clicked(object o, EventArgs args) 
		{
			if(RoomPicker.SelectedIndex < 0)
			{
				await DisplayAlert("Pomieszczenie", "Wybierz pomieszczenie", "OK");
				return;
			}

			RoomEntity selectedRoom = null;
			string selectedName = RoomPicker.Items[RoomPicker.SelectedIndex];

			foreach (var room in rooms)
			{
				if(room.name == selectedName)
				{
					selectedRoom = room;
					break;
				}
			}

			if(selectedRoom != null)
			{
				App.Current.MainPage = new NavigationPage(new ScanItemPage(selectedRoom));
			}
			else
			{
				await DisplayAlert("Błąd", "Błąd niespodzianka, nie znaleziono wybranego pomieszczenia", "OK");
			}
		}

		private async void onApiError(object o, ErrorEventArgs error)
		{
			await DisplayAlert("Błąd", error.MessageForUser, "OK");
		}


		private void Return_button_clicked(object o, EventArgs e)
		{
			App.Current.MainPage = new WelcomeViewPage();
		}
		
		public void AddRoom_clicked(object o, EventArgs args)
		{
			App.Current.MainPage = new AddRoom();
		}
		public void AddBuildingClicked(object o, EventArgs e)
		{
			App.Current.MainPage = new AddBuildingView();
		}

		public void RoomPicker_SelectedIndexChanged(object o, EventArgs e)
		{
			if (RoomPicker.SelectedItem == null)
			{
				ContinueBtn.IsEnabled = false;
			}
			else
			{
				ContinueBtn.IsEnabled = true;
			}
		}
	}
}