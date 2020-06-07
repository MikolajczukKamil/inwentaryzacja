using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Inwentaryzacja.views.view_chooseRoom;
using System;
using System.Linq;
using System.Threading;
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
		public bool addedNewBuilding = false;
		public bool addedNewRoom = false;

		APIController api = new APIController();

		public ChooseRoomPage()
		{
			InitializeComponent();
			api.ErrorEventHandler += onApiError;
			BindingContext = this;
		}

		protected override void OnAppearing()
		{
			if(BuildingPicker.Items.Count==0 || addedNewBuilding)
			{
				GetBuildings();
			}
			if(addedNewRoom)
			{
				BuildingPicker_SelectedIndexChanged(this, null);
				addedNewRoom = false;
			}
			
			base.OnAppearing();
		}

		private async Task<PermissionStatus> CheckPermissions()
		{
			var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

			if (status != PermissionStatus.Granted)
			{
				status = await Permissions.RequestAsync<Permissions.Camera>();
			}

			return status;
		}

		private void BuildingPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(BuildingPicker.SelectedIndex >= 0 && BuildingPicker.SelectedIndex < BuildingPicker.Items.Count)
			{
				string choosenBuildingName = BuildingPicker.Items[BuildingPicker.SelectedIndex];
				GetBuildingRooms(choosenBuildingName);
			}
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
			EnableView(false);
			rooms = await api.getRooms(buildingId);
			EnableView(true);

			if (rooms == null)
			{
				return;
			} 

			if (RoomPicker.Items.Count > 0) RoomPicker.Items.Clear();

			foreach (RoomEntity item in rooms)
			{
				RoomPicker.Items.Add(item.name);
			}

			if (RoomPicker.Items.Count > 0)
			{
				RoomPicker.Placeholder = "Sala";
				RoomPicker.IsEnabled = true;
			} 
			else
			{
				await DisplayAlert("Uwaga", "W tym budynku nie ma żadnej sali!", "OK");
				RoomPicker.Placeholder = "Brak sal dla tego budynku!";
				RoomPicker.IsEnabled = false;
			} 
		}		
		
		private async void GetBuildings()
		{
			EnableView(false);
			buildings = await api.getBuildings();

			if (buildings == null)
			{
				EnableView(true);
				return;
			}

			BuildingPicker.Items.Clear();

			foreach (BuildingEntity item in buildings)
			{
				BuildingPicker.Items.Add(item.name);
			}

			if (BuildingPicker.Items.Count > 0)
			{
				if (addedNewBuilding)
				{
					BuildingPicker.SelectedItem = BuildingPicker.Items[BuildingPicker.Items.Count - 1];
					addedNewBuilding = false;
				}
				else
				{
					BuildingPicker.SelectedItem = BuildingPicker.Items[0];
				}
				
			}

			EnableView(true);
		}

		private void EnableView(bool state)
		{
			IsBusy = !state;
			RoomPicker.IsEnabled = state;
			BuildingPicker.IsEnabled = state;
			BackBtn.IsEnabled = state;
			AddBuildingBtn.IsEnabled = state;
			AddRoomBtn.IsEnabled = state;
			LogoutButton.IsEnabled = state;

			if(state==false)
			{
				ContinueBtn.IsEnabled = false;
			}
			else
			{
				RoomPicker_SelectedIndexChanged(this, null);
			}
		}

		private async void Continue_Button_Clicked(object o, EventArgs args) 
		{
			EnableView(false);

			if (RoomPicker.SelectedIndex < 0)
			{
				await DisplayAlert("Pomieszczenie", "Wybierz pomieszczenie", "OK");
				EnableView(false);
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
				var status = await CheckPermissions();

				if (status != PermissionStatus.Granted)
				{
					await DisplayAlert("Komunikat","Bez uprawnień do kamery aplikacja nie może działać poprawnie", "OK");
				}
				else
				{
					await Navigation.PushModalAsync(new ScanItemPage(selectedRoom));
				}
			}
			else
			{
				await DisplayAlert("Błąd", "Błąd niespodzianka, nie znaleziono wybranego pomieszczenia", "OK");
			}

			EnableView(true);
		}

		private async void onApiError(object o, ErrorEventArgs error)
		{
			await DisplayAlert("Błąd", error.MessageForUser, "OK");

			if(!error.Auth)
			{
				await Navigation.PushAsync(new LoginPage());
			}
		}


		private async void Return_button_clicked(object o, EventArgs e)
		{
			EnableView(false);
			await Navigation.PopAsync();
			EnableView(true);
		}
		
		public async void AddRoom_clicked(object o, EventArgs args)
		{
			EnableView(false);
			await Navigation.PushAsync(new AddRoom(buildings, BuildingPicker.SelectedIndex));	
			EnableView(true);
		}

		public async void AddBuildingClicked(object o, EventArgs e)
		{
			EnableView(false);
			await Navigation.PushAsync(new AddBuildingView());
			EnableView(true);
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

		private async void LogoutButtonClicked(object sender, EventArgs e)
		{
			EnableView(false);
			if (await DisplayAlert("Wylogowywanie", "Czy na pewno chcesz się wylogować?", "Tak", "Nie"))
			{
				var session = new SessionController(new APIController());
				session.RemoveSession();
				App.Current.MainPage = new LoginPage();
			}
			EnableView(true);
		}
	}
}
