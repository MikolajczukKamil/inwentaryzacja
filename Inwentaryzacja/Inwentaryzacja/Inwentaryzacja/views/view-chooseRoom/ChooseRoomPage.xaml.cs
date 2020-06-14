using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Inwentaryzacja.views.Helpers;
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

	/// <summary>
	/// Klasa odpowiadajaca za widok okna wybrania pokoju
	/// </summary>
	public partial class ChooseRoomPage : ContentPage
	{
		RoomEntity[] rooms;
		BuildingEntity[] buildings;
		public bool addedNewBuilding = false;
		public bool addedNewRoom = false;

		APIController api = new APIController();

		/// <summary>
		/// Konstruktor klasy
		/// </summary>
		public ChooseRoomPage()
		{
			InitializeComponent();
			api.ErrorEventHandler += onApiError;
			BindingContext = this;
		}

		/// <summary>
		/// Funkcja odpowiadajaca za wyswietlenie budynkow po przejsciu do okna
		/// </summary>
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

		/// <summary>
		/// Funkcja odpowiadajaca za sprawdzenie permisji dostepu do kamery uzytkownika 
		/// </summary>
		/// <returns>status permisji</returns>
		private async Task<PermissionStatus> CheckPermissions()
		{
			var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

			if (status != PermissionStatus.Granted)
			{
				status = await Permissions.RequestAsync<Permissions.Camera>();
			}

			return status;
		}

		/// <summary>
		/// Funkcja odpowiadajaca za zmiane wybranego budynku
		/// </summary>
		private void BuildingPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(BuildingPicker.SelectedIndex >= 0 && BuildingPicker.SelectedIndex < BuildingPicker.Items.Count)
			{
				string choosenBuildingName = BuildingPicker.Items[BuildingPicker.SelectedIndex];
				GetBuildingRooms(choosenBuildingName);
			}
		}

		/// <summary>
		/// Funkcja odpowiadajaca za zwrocenie pokojow w danym budynku
		/// </summary>
		/// <param name="name">nazwa budynku</param>
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

		/// <summary>
		/// Funkcja odpowiadajaca za zwrocenie pokojow w danym budynku
		/// </summary>
		/// <param name="buildingId">ID budynku dla ktorego zwracamy pokoje</param>
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
				RoomPicker.Placeholder = "Brak sal dla tego budynku!";
				RoomPicker.IsEnabled = false;
			} 
		}

		/// <summary>
		/// Funkcja odpowiadajaca za zwrocenie budynkow
		/// </summary>
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

		/// <summary>
		/// Funkcja odpowiadajaca za umozliwienie wyswietlenia widoku okna
		/// </summary>
		/// <param name="state">stan okna</param>
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

		/// <summary>
		/// Funkcja odpowiadajaca za obsluge przycisku kontynuowania
		/// </summary>
		private void Continue_Button_Clicked(object o, EventArgs args) 
		{
			EnableView(false);

			Device.BeginInvokeOnMainThread(async () =>
			{
				if (RoomPicker.SelectedIndex < 0)
				{
					await DisplayAlert("Pomieszczenie", "Wybierz pomieszczenie", "OK");
					EnableView(true);
					return;
				}

				RoomEntity selectedRoom = null;
				string selectedName = RoomPicker.Items[RoomPicker.SelectedIndex];

				await Task.Run(() => 
				{ 
					foreach (var room in rooms)
					{
						if (room.name == selectedName)
						{
							selectedRoom = room;
							break;
						}
					}
				});

				if (selectedRoom != null)
				{
					var status = await CheckPermissions();

					if (status != PermissionStatus.Granted)
					{
						await DisplayAlert("Komunikat", "Bez uprawnień do kamery aplikacja nie może działać poprawnie", "OK");
					}
					else
					{
						var scans = await api.getScans();
						ScanEntity existingScan = null;

						if(scans != null && scans.Length > 0)
						{
							foreach (var scan in scans)
							{
								if(scan.room.id == selectedRoom.id)
								{
									existingScan = scan;
								}
							}
						}

						if (existingScan != null)
						{
							bool useThisScan = await DisplayAlert("Znaleziono niedokończone skanowanie", "Czy chcesz użyć niedokończonego skanowania?", "Tak", "Nie");

							if(!useThisScan)
							{
								var scanning = new ScanningUpdate(api, selectedRoom, existingScan.id);
								scanning.Delete();
								existingScan = null;
							}
						}

						int scanId = existingScan != null ? existingScan.id : await api.addScan(new ScanPrototype(selectedRoom.id));

						await Navigation.PushModalAsync(new ScanItemPage(selectedRoom, scanId, existingScan));
					}
				}
				else
				{
					await DisplayAlert("Błąd", "Błąd niespodzianka, nie znaleziono wybranego pomieszczenia", "OK");
				}

				EnableView(true);
			});
		}

		/// <summary>
		/// Funkcja odpowiadajaca za wyswietlenie bledu
		/// </summary>
		private async void onApiError(object o, ErrorEventArgs error)
		{
			await DisplayAlert("Błąd", error.MessageForUser, "OK");

			if(!error.Auth)
			{
				await Navigation.PushAsync(new LoginPage());
			}
		}

		/// <summary>
		/// Funkcja odpowiadajaca za obsluge przycisku powrotu
		/// </summary>
		private async void Return_button_clicked(object o, EventArgs e)
		{
			EnableView(false);
			await Navigation.PopAsync();
			EnableView(true);
		}

		/// <summary>
		/// Funkcja odpowiadajaca za obsluge przycisku dodania pokoju
		/// </summary>
		public async void AddRoom_clicked(object o, EventArgs args)
		{
			EnableView(false);
			await Navigation.PushAsync(new AddRoom(buildings, BuildingPicker.SelectedIndex));	
			EnableView(true);
		}

		/// <summary>
		/// Funkcja odpowiadajaca za obsluge przycisku dodania budynku
		/// </summary>
		public async void AddBuildingClicked(object o, EventArgs e)
		{
			EnableView(false);
			await Navigation.PushAsync(new AddBuildingView());
			EnableView(true);
		}

		/// <summary>
		/// Funkcja odpowiadajaca za zmiane wybranego pokoju 
		/// </summary>
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

		/// <summary>
		/// Funkcja odpowiadajaca za obsluge przycisku wylogowania
		/// </summary>
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
