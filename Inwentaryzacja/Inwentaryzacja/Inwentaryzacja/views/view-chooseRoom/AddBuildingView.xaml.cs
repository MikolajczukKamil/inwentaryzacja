﻿using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        APIController api = new APIController();

        public AddBuildingView()
        {
            InitializeComponent();
            api.ErrorEventHandler += onApiError;
        }

        public async void AddButtonClicked(object o, EventArgs e)
        {
            string name = BuildingName.Text;
            bool isAlreadyAdded = false;
            bool isCreated=false;

            BuildingEntity[] buildings = await api.getBuildings();
           
            foreach (BuildingEntity item in buildings)
            {
                if (name == item.name)
                {
                    await DisplayAlert("Dodawanie budynku", "Taki budynek już istnieje.", "Wyjdź");
                    isAlreadyAdded = true;

                    return;
                }
            }

            if (!isAlreadyAdded)
            {
                isCreated = await api.createBuilding(new BuildingPrototype(name));
            }

            await PopupNavigation.Instance.PopAsync(true);

            if (isCreated) {
                await DisplayAlert("Dodawanie budynku", "Pomyślnie dodano nowy budynek", "Wyjdź");
            }
            else {
                await DisplayAlert("Dodawanie budynku", "Niepowodzenie podczas dodawania budynku", "Wyjdź");
            } 
        }

        private async void onApiError(object o, ErrorEventArgs error)
        {
            await DisplayAlert("Błąd", error.MessageForUser, "Wyjdz");
        }
    }
}