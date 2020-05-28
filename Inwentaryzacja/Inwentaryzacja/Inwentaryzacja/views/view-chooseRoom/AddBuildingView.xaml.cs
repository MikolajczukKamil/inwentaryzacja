using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using System.Threading.Tasks;
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
            BindingContext = this;
        }

        public async void AddButtonClicked(object o, EventArgs e)
        {
            string name = BuildingName.Text;
            Task<BuildingEntity[]> buildingsTask = api.getBuildings();
            EnableView(false);
            await buildingsTask;
            EnableView(true);
            BuildingEntity[] buildings = buildingsTask.Result;

            foreach (BuildingEntity item in buildings)
            {
                if (name == item.name)
                {
                    await DisplayAlert("Dodawanie budynku", "Taki budynek już istnieje.", "OK");
                    return;
                }
            }

            Task<bool> createTask = api.createBuilding(new BuildingPrototype(name));
            EnableView(false);
            await createTask;
            EnableView(true);
            bool isCreated = createTask.Result;

            App.Current.MainPage = new ChooseRoomPage(true);

            if (isCreated)
            {
                await DisplayAlert("Dodawanie budynku", "Pomyślnie dodano nowy budynek", "OK");
            }
        }

        private void EnableView(bool state)
        {
            IsBusy = !state;
            AddBtn.IsEnabled = state;
            BackBtn.IsEnabled = state;
        }

        private async void onApiError(object o, ErrorEventArgs error)
        {
            await DisplayAlert("Dodawanie budynku", error.MessageForUser, "OK");
        }

        private void return_ChooseRoom(object o, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}