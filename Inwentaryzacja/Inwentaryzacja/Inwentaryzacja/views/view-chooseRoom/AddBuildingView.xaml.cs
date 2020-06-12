using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_chooseRoom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// Klasa odpowiadajaca za widok okna dodania budynku
    /// </summary>
    public partial class AddBuildingView : ContentPage
    {
        APIController api = new APIController();
        /// <summary>
		/// Konstruktor klasy
		/// </summary>
        public AddBuildingView()
        {
            InitializeComponent();
            api.ErrorEventHandler += onApiError;
            BindingContext = this;
        }

        /// <summary>
		/// Funkcja odpowiadajaca za obsluge przycisku dodawania budynku
		/// </summary>
        public async void AddButtonClicked(object o, EventArgs e)
        {
            EnableView(false);
            string name = BuildingName.Text;
            BuildingEntity[] buildings = await api.getBuildings();

            if (buildings == null)
            {
                EnableView(true);
                return;
            }
                

            foreach (BuildingEntity item in buildings)
            {
                if (name == item.name)
                {
                    await DisplayAlert("Dodawanie budynku", "Taki budynek już istnieje.", "OK");
                    return;
                }
            }

            int isCreated = await api.createBuilding(new BuildingPrototype(name));

            if (isCreated>0)
            {
                var stack = Navigation.NavigationStack;
                var previousPage = (ChooseRoomPage)stack[stack.Count - 2];
                previousPage.addedNewBuilding = true;
                await Navigation.PopAsync();
                await DisplayAlert("Dodawanie budynku", "Pomyślnie dodano nowy budynek", "OK");
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
            AddBtn.IsEnabled = state;
            BackBtn.IsEnabled = state;
        }
        /// <summary>
		/// Funkcja odpowiadajaca za wyswietlenie bledu
		/// </summary>
        private async void onApiError(object o, ErrorEventArgs error)
        {
            await DisplayAlert("Błąd", error.MessageForUser, "OK");

            if (error.Auth == false)
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }

        /// <summary>
		/// Funkcja odpowiadajaca za wybranie pokoju
		/// </summary>
        private void return_ChooseRoom(object o, EventArgs e)
        {
            EnableView(false);
            Navigation.PopAsync();
            EnableView(true);
        }

        /// <summary>
		/// Funkcja odpowiadajaca za obsluge przycisku wylogowania
		/// </summary>
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