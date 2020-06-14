using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_chooseRoom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    /// <summary>
    /// Klasa odpowiadajaca za widok okna dodania pokoju
    /// </summary>
    public partial class AddRoom : ContentPage
    {
        BuildingEntity[] buildings;
        int indexSelectedItem;
        APIController api = new APIController();

        /// <summary>
		/// Konstruktor klasy
		/// </summary>
        public AddRoom(BuildingEntity[] buildings, int indexSelectedItem = 0)
        {
            this.buildings = buildings;
            this.indexSelectedItem = indexSelectedItem;
            InitializeComponent();
            api.ErrorEventHandler += onApiError;
            BindingContext = this;
        }

        /// <summary>
		/// Funkcja odpowiadajaca za wyswietlenie budynkow po przejsciu do okna
		/// </summary>
        protected override void OnAppearing()
        {
            GetBuildings();
            base.OnAppearing();
        }

        /// <summary>
		/// Funkcja odpowiadajaca za zwrocenie budynkow
		/// </summary>
        private async void GetBuildings()
        {
            EnableView(false);

            if (buildings == null)
            {
                buildings = await api.getBuildings();
            }

            if (buildings == null)
            {
                EnableView(true);
                return;
            }

            foreach (BuildingEntity item in buildings)
            {
                BuildingPicker.Items.Add(item.name);
            }

            if (BuildingPicker.Items.Count > 0)
            {
                if (indexSelectedItem >=0 && indexSelectedItem < BuildingPicker.Items.Count)
                {
                    BuildingPicker.SelectedItem = BuildingPicker.Items[indexSelectedItem];
                }
                else
                {
                    BuildingPicker.SelectedItem = BuildingPicker.Items[0];
                }
            }

            EnableView(true);
        }

        /// <summary>
        /// Funkcja odpowiadajaca za wybranie pokoju
        /// </summary>
        public async void return_ChooseRoom(object o, EventArgs args)
        {
            EnableView(false);
            await Navigation.PopAsync();
            EnableView(true);
        }
        /// <summary>
        /// Funkcja odpowiadajaca za sprawdzenie pokoju
        /// </summary>
        public async void Check_Room(object o, EventArgs args)
        {
            EnableView(false);

            string number = room_number.Text;

            BuildingEntity mybuilding = new BuildingEntity();

            string choosenBuildingName = "";

            if (BuildingPicker.Items.Count > 0) {
                
                choosenBuildingName = BuildingPicker.Items[BuildingPicker.SelectedIndex];
            }

            if(buildings != null)
            {
                foreach (var item in buildings)
                {
                    if (item.name == choosenBuildingName)
                    {
                        mybuilding = item;
                    }
                }
            }

            int roomId = await api.createRoom(new RoomPropotype(number, mybuilding));

            if (roomId > 0)
            {
                var stack = Navigation.NavigationStack;
                var previousPage = (ChooseRoomPage)stack[stack.Count - 2];
                previousPage.addedNewRoom = true;
                await Navigation.PopAsync();
                await DisplayAlert("Dodawanie pokoju", "Pomyślnie dodano nowy pokój", "OK");
            }

            EnableView(true);
        }
        /// <summary>
		/// Funkcja odpowiadajaca za umozliwienie wyswietlenia widoku okna
        /// <param name="state">stan okna</param>
		/// </summary>
        private void EnableView(bool state)
        {
            IsBusy = !state;
            AddRoomBtn.IsEnabled = state;
            BackBtn.IsEnabled = state;
            LogoutButton.IsEnabled = state;
        }
        /// <summary>
        /// Funkcja odpowiadajaca za wyswietlenie bledu
        /// </summary>
        private async void onApiError(object o, ErrorEventArgs error)
        {
            await DisplayAlert("Dodawanie pokoju", error.MessageForUser, "OK");

            if (error.Auth == false)
            {
                await Navigation.PushAsync(new LoginPage());
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
