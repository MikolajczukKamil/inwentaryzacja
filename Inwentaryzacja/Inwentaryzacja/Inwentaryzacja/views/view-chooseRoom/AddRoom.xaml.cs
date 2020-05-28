using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            GetBuildings();
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
  
        public void return_ChooseRoom(object o, EventArgs args)
        {
            Application.Current.MainPage = new NavigationPage(new ChooseRoomPage());
        }
       
        public async void Check_Room(object o, EventArgs args)
        {
            string number = room_number.Text;
            BuildingEntity[] allbuildings = await api.getBuildings();
            BuildingEntity mybuilding = new BuildingEntity();
            string choosenBuildingName = BuildingPicker.Items[BuildingPicker.SelectedIndex];
            foreach (var item in allbuildings)
            {
                if (item.name == choosenBuildingName)
                {
                    mybuilding = item;
                }
            }

            RoomEntity[] rooms = await api.getRooms(mybuilding.id);
            bool roomexist = false;

            foreach (var item in rooms)
            {
                if (item.name == number)
                {
                    roomexist = true;
                    await DisplayAlert("Błąd", "W tym budynku istnieje już taki pokój", "Wyjdź");
                }
            }

            if (!roomexist)
            {
                Task<bool> createTask = api.createRoom(new RoomPropotype(number, mybuilding));
                EnableView(false);
                await createTask;
                EnableView(true);
                bool isCreated = createTask.Result;
               
                App.Current.MainPage = new ChooseRoomPage(true);

                if (isCreated)
                {
                    await DisplayAlert("Dodawanie pokoju", "Pomyślnie dodano nowy pokój", "OK");
                }
              
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
        }

    }
}