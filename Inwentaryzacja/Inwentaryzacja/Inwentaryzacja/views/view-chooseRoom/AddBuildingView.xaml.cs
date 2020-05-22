using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class AddBuildingView : PopupPage
    {
        APIController api;

        public AddBuildingView()
        {
            InitializeComponent();
            api = new APIController();
        }
        public async void AddButtonClicked(object o, EventArgs e)
        {
            string name = BuildingName.Text;

            BuildingPrototype buildingPrototype = new BuildingPrototype(name);

            bool isAdded = await api.createBuilding(buildingPrototype);


            //PopupNavigation.Instance.PopAsync(true);

            if (isAdded == true) DisplayAlert("Dodawanie budynku", "Pomyślnie dodano nowy budynek", "Wyjdź");
            else DisplayAlert("Dodawanie budynku", "Niepowodzenie podczas dodawania budynku", "Wyjdź");
        }
    }
}