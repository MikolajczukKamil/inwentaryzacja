using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_chooseRoom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBuildingView : ContentPage
    {
        APIController api = new APIController();

        public AddBuildingView()
        {
            InitializeComponent();
            api.ErrorEventHandler += onApiError;
        }

        public async void AddButtonClicked(object o, EventArgs e)
        {
            string name = BuildingName.Text;
            BuildingEntity[] buildings = await api.getBuildings();

            foreach (BuildingEntity item in buildings)
            {
                if (name == item.name)
                {
                    await DisplayAlert("Dodawanie budynku", "Taki budynek już istnieje.", "Wyjdź");
                    return;
                }
            }

            bool isCreated = await api.createBuilding(new BuildingPrototype(name));

            App.Current.MainPage = new ChooseRoomPage();

            if (isCreated)
            {
                await DisplayAlert("Dodawanie budynku", "Pomyślnie dodano nowy budynek", "Wyjdź");
            }
        }

        private async void onApiError(object o, ErrorEventArgs error)
        {
            await DisplayAlert("Dodawanie budynku", error.MessageForUser, "Wyjdz");
        }

        private void return_ChooseRoom(object o, EventArgs e)
        {
            App.Current.MainPage = new ChooseRoomPage();
        }
    }
}