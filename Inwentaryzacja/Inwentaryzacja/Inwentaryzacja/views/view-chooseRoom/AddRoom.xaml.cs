using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_chooseRoom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRoom : ContentPage
    {
        APIController api = new APIController();
        
        protected async override void OnAppearing()
        {
            
            base.OnAppearing();
            List<B> buildings_list = new List<B>();

            BuildingEntity[] build = await api.getBuildings();

            for (int i = 0; i < build.Length; i++)
            {
                buildings_list.Add(new B() {BuildingName= build[i].name });
            }

            Building_List.ItemsSource = buildings_list;
        }

        public class B
        {
            public string BuildingName { get; set; }
        }

        public AddRoom()
        {
            InitializeComponent();
        }

        public async void return_ChooseRoom(object o, EventArgs args)
        {
            await Navigation.PopAsync();
        }
       
        public async void Check_Room(object o, EventArgs args)
        {
            string number = room_number.Text;
            B budynek = (B) Building_List.SelectedItem;
            BuildingEntity[] buildings = await api.getBuildings();
            BuildingEntity mybuilding = new BuildingEntity();

            foreach (var item in buildings)
            {
                if (item.name == budynek.BuildingName)
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
                RoomPropotype roomprop = new RoomPropotype(number, mybuilding);
                bool isAdded = await api.createRoom(roomprop);

                if (isAdded)
                {
                    await DisplayAlert("Dodawanie pokoju", "Pomyślnie dodano nowy pokój", "Wyjdź");
                }
                else
                {
                    await DisplayAlert("Dodawanie pokoju", "Niepowodzenie podczas dodawania pokoju", "Wyjdź");
                }
            }
        }
    }
}