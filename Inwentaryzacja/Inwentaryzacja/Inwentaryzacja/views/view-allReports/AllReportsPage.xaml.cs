using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Inwentaryzacja.views.view_allReports;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllReportsPage : ContentPage
    {
        ReportHeaderEntity[] reportHeaders;

        APIController api = new APIController();

        public AllReportsPage()
        {
            InitializeComponent();
            api.ErrorEventHandler += onApiError;
            BindingContext = this;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            EnableView(false);

            List<AllReport> allReportList = new List<AllReport>();
            reportHeaders = await api.getReportHeaders();

            if (reportHeaders == null) return;

            for (int i = 0; i < reportHeaders.Length; i++)
            {
                allReportList.Add(new AllReport() { ReportName = reportHeaders[i].name, ReportRoom = reportHeaders[i].room.name, ReportDate = Convert.ToString(reportHeaders[i].create_date) });
            }

            ReportList.ItemsSource = allReportList;

            EnableView(true);
        }

        private async void onApiError(object o, ErrorEventArgs error)
        {
            await DisplayAlert("Błąd", error.MessageForUser, "Wyjdz");
        }

        public class AllReport
        {
            public string ReportName { get; set; }
            public string ReportRoom { get; set; }
            public string ReportDate { get; set; }
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
        }

        private async void ReportList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            EnableView(false);

            AllReport selectedReport = (AllReport)ReportList.SelectedItem;

            ReportHeaderEntity reportHeaderEntity = null;
            ReportPositionEntity[] reportPositionEntities;

            if (selectedReport == null) return;
            foreach (ReportHeaderEntity item in reportHeaders)
            {
                if (item.name == selectedReport.ReportName)
                {
                    reportHeaderEntity = item;
                }
            }
            if (reportHeaderEntity == null) return;

            Task<ReportPositionEntity[]> reportPositionTask = api.getReportPositions(reportHeaderEntity.id);
            
            await reportPositionTask;
            reportPositionEntities = reportPositionTask.Result;
            if (reportPositionEntities == null) return;

            string[] counted = GetScannedItemsCount(reportPositionEntities, reportHeaderEntity.room);

            string inThisRoom = counted[0];
            string moveToRoom = counted[1];
            string moveFromRoom = counted[2];
            string inAnotherRoom = counted[3];
            string scannedAll = counted[4];

            string headerText = reportHeaderEntity.name;
            string roomText = reportHeaderEntity.room.name;
            DateTime date = reportHeaderEntity.create_date;
            string editedDay = date.Day < 10 ? "0" + date.Day : "" + date.Day;
            string editedMonth = date.Month < 10 ? "0" + date.Month : "" + date.Month;
            string createDate = editedDay + "." + editedMonth + "." + date.Year + "r.";
            string createTime = date.TimeOfDay.ToString();
            string ownerText = reportHeaderEntity.owner.login;

            EnableView(true);
            App.Current.MainPage = new ReportDetailsView(headerText, roomText, createDate, createTime, ownerText, inThisRoom, moveToRoom, moveFromRoom, inAnotherRoom, scannedAll);
        }

        private string[] GetScannedItemsCount(ReportPositionEntity[] reportPositionEntities, RoomEntity currentRoom)
        {
            string[] result = new string[5];

            Dictionary<string, int> inThisRoomCount = new Dictionary<string, int>();
            Dictionary<string, int> movedToRoomCount = new Dictionary<string, int>();
            Dictionary<string, int> movedFromRoomCount = new Dictionary<string, int>();
            Dictionary<string, int> inAnotherRoomCount = new Dictionary<string, int>();
            Dictionary<string, int> scannedAll = new Dictionary<string, int>();

            foreach (ReportPositionEntity item in reportPositionEntities)
            {
                
                string typeName = item.asset.type.name;
                if (item.present == true)
                {
                    if (!scannedAll.ContainsKey(typeName))
                    {
                        scannedAll.Add(typeName, 1);
                    }
                    else
                    {
                        scannedAll[typeName]++;
                    }
                }
                if (item.present == true && item.previous_room == currentRoom)
                {
                    if (!inThisRoomCount.ContainsKey(typeName))
                    {
                        inThisRoomCount.Add(typeName, 1);
                    }
                    else
                    {
                        inThisRoomCount[typeName]++;
                    }
                }

                else if (item.present == true && item.previous_room != currentRoom)
                {
                    if (!movedToRoomCount.ContainsKey(typeName))
                    {
                        movedToRoomCount.Add(typeName, 1);
                    }
                    else
                    {
                        movedToRoomCount[typeName]++;
                    }
                }
                else if (item.present == false && item.previous_room == currentRoom)
                {
                    if (!movedFromRoomCount.ContainsKey(typeName))
                    {
                        movedFromRoomCount.Add(typeName, 1);
                    }
                    else
                    {
                        movedFromRoomCount[typeName]++;
                    }
                }
                else if (item.present == false && item.previous_room != currentRoom)
                { 
                    if (!inAnotherRoomCount.ContainsKey(typeName))
                    {
                        inAnotherRoomCount.Add(typeName, 1);
                    }
                    else
                    {
                        inAnotherRoomCount[typeName]++;
                    }
                }
            }

            result[0] = GenerateString(inThisRoomCount);
            result[1] = GenerateString(movedToRoomCount);
            result[2] = GenerateString(movedFromRoomCount);
            result[3] = GenerateString(inAnotherRoomCount);
            result[4] = GenerateString(scannedAll);

            return result;
        }

        private string GenerateString(Dictionary<string, int> dict)
        {
            string result = "";

            if (dict.Count == 0 || dict == null) return result;

            foreach (KeyValuePair<string,int> item in dict)
            {
                string dopisek = "sztuk";
                string spacebars = "";
                int spaceCounter = 12 - item.Key.Length;

                if (item.Key == "krzesło") spaceCounter += 3;
                if (item.Key == "monitor") spaceCounter += 3;
                if (item.Key == "stół") spaceCounter += 7;
                if (item.Key == "tablica") spaceCounter += 4;
                if (item.Key == "projektor") spaceCounter += 3;

                for (int i = 0; i < spaceCounter; i++) spacebars += " ";                

                if (item.Value == 1) dopisek = "sztuka";
                if (item.Value == 2|| item.Value == 3|| item.Value == 4) dopisek = "sztuki";

                result += item.Key + spacebars + item.Value + " " + dopisek +Environment.NewLine;
            }

            return result;
        }

        private void EnableView(bool state)
        {
            IsBusy = !state;
            ReportList.IsEnabled = state;
        }
    }
}