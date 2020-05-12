using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inwentaryzacja;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        APIController api;
        public LoginPage()
        {
            api = new APIController();
            api.ErrorEventHandler += LoginFail;
            InitializeComponent();
        }

        private async void _loginButton_Clicked(object sender, EventArgs e)
        {
            if(await api.LoginUser(_login.Text, _password.Text))
            {
                await DisplayAlert("Logowanie", "SUKCES!!", "OK");

                // Example();

                if(Navigation.NavigationStack.Count == 0)
                {
                    App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
                }
                else
                {
                    await Navigation.PopAsync();
                }  
            }
        }

        private async void LoginFail(object sender, ErrorEventArgs e)
        {
            await DisplayAlert("Błąd logowania", e.MessageForUser, "OK");
        }

        async void Example()
        {
#if true // Testy manualne api, "user1", "111"
            //var getBuildings = await api.getBuildings();
            //var getRooms = await api.getRooms(1);
            //var getAssetInfo = await api.getAssetInfo(1);
            //var getAssetInfo2 = await api.getAssetInfo(3);
            //var CreateAsset = await api.CreateAsset(new Models.AssetPrototype(new Models.AssetType(1, "", 'c')));
            //var createBuilding = await api.createBuilding(new Models.BuildingPrototype("nowy"));
            //var createRoom = await api.createRoom(new Models.RoomPropotype("nowy", new Models.Building(1, "")));
            //var getReportHeaders = await api.getReportHeaders(); // Date
            //var getReportHeader = await api.getReportHeader(5); // Date
            //var getReportPositions = await api.getReportPositions(9);
            //var createReport = await api.createReport(new ReportPrototype(
            //    "nowy raport createReport",
            //    new Room(1, "", null),
            //    (new ReportPositionPrototype[] { new ReportPositionPrototype(new Asset(50, null), null, true) })
            //    ));
#endif
        }
    }
}