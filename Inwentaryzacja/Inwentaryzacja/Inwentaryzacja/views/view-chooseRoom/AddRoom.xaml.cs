using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_chooseRoom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRoom : ContentPage
    {
        APIController api = new APIController();


        public AddRoom()
        {
            InitializeComponent();
        }

        public void return_ChooseRoom(object o, EventArgs args)
        {
            Application.Current.MainPage = new NavigationPage(new ChooseRoomPage());
        }

        public async void Check_Room(object o, EventArgs args)
        {
            string number = room_number.Text;
            string nazwa = building_name.Text;
            bool buildingexist = false;
           
            BuildingEntity[] buildings = await api.getBuildings();

            foreach (var item in buildings)
            {
                if (item.name == nazwa)
                {
                    buildingexist = true;
                    RoomPropotype roomprop = new RoomPropotype(number, item);
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

            if (!buildingexist)
            {
                await DisplayAlert("Brak Budynku", "Nie istnieje taki budynek", "Wyjdź");
            }
        }
    }
}