using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_chooseRoom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRoom : ContentPage
    {
        BuildingEntity[] buildings;
        bool addedNewBuilding = false;
        APIController api = new APIController();

        public AddRoom()
        {
            InitializeComponent();
            api.ErrorEventHandler += onApiError;
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            GetBuildings();
            base.OnAppearing();
        }

        private async void GetBuildings()
        {
            EnableView(false);

            buildings = await api.getBuildings();

            EnableView(true);

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
  
        public async void return_ChooseRoom(object o, EventArgs args)
        {
            await Navigation.PopAsync();
        }
       
        public async void Check_Room(object o, EventArgs args)
        {
            string number = room_number.Text;

            BuildingEntity mybuilding = new BuildingEntity();
            string choosenBuildingName = BuildingPicker.Items[BuildingPicker.SelectedIndex];

            foreach (var item in buildings)
            {
                if (item.name == choosenBuildingName)
                {
                    mybuilding = item;
                }
            }

            EnableView(false);

            bool isCreated = await api.createRoom(new RoomPropotype(number, mybuilding));

            EnableView(true);

            if (isCreated)
            {
                Navigation.PopAsync();
                await DisplayAlert("Dodawanie pokoju", "Pomyślnie dodano nowy pokój", "OK");
            }
        }
        
        private void EnableView(bool state)
        {
            IsBusy = !state;
            AddRoomBtn.IsEnabled = state;
            BackBtn.IsEnabled = state;
        }
      
        private async void onApiError(object o, ErrorEventArgs error)
        {
            await DisplayAlert("Dodawanie pokoju", error.MessageForUser, "OK");

            if (error.Auth == false)
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }

        private async void LogoutButtonClicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Wylogowywanie", "Czy na pewno chcesz się wylogować?", "Tak", "Nie"))
            {
                var session = new SessionController(new APIController());
                session.RemoveSession();
                App.Current.MainPage = new LoginPage();
            }
        }
    }
}
