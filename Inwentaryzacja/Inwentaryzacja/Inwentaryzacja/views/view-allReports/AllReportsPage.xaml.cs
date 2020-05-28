using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Inwentaryzacja.views.view_allReports;
using System;
using System.Collections.Generic;
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
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            List<AllReport> allReportList = new List<AllReport>();

            reportHeaders = await api.getReportHeaders();

            if (reportHeaders == null) return;

            for (int i = 0; i < reportHeaders.Length; i++)
            {
                allReportList.Add(new AllReport() { ReportName = reportHeaders[i].name, ReportRoom = reportHeaders[i].room.name, ReportDate = Convert.ToString(reportHeaders[i].create_date) });
            }

            ReportList.ItemsSource = allReportList;
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
            AllReport selectedReport = (AllReport)ReportList.SelectedItem;

            ReportHeaderEntity reportHeaderEntity = null;
            ReportPositionEntity[] reportPositionEntities;
            AssetEntity[] assetsInRoom;

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

            Task<AssetEntity[]> assetsTask = api.getAssetsInRoom(reportHeaderEntity.room.id);
            await assetsTask;
            assetsInRoom = assetsTask.Result;
            if (assetsInRoom == null) return;

            //--------------------------
            string listedAssets = GetScannedItemsCount(reportPositionEntities, assetsInRoom, reportHeaderEntity.room.name);
            string headerText = reportHeaderEntity.name;
            string roomText = reportHeaderEntity.room.name;
            DateTime date = reportHeaderEntity.create_date;
            string editedDay = date.Day < 10 ? "0" + date.Day : "" + date.Day;
            string editedMonth = date.Month < 10 ? "0" + date.Month : "" + date.Month;
            string createDate = editedDay + "." + editedMonth + "." + date.Year + "r.";
            string createTime = date.TimeOfDay.ToString();
            string ownerText = reportHeaderEntity.owner.login;

            App.Current.MainPage = new ReportDetailsView(headerText, roomText, createDate, createTime, ownerText, listedAssets);
        }

        private string GetScannedItemsCount(ReportPositionEntity[] reportPositionEntities, AssetEntity[] assetsInRoom, string currentRoom)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Dictionary<string, int> scannedAssetsCounts = new Dictionary<string, int>();
            Dictionary<string, int> roomAssetsCounts = new Dictionary<string, int>();

            foreach (ReportPositionEntity item in reportPositionEntities)
            {
                if (item.present == true)
                {
                    string typeName = item.asset.type.name;
                    if (!scannedAssetsCounts.ContainsKey(typeName))
                    {
                        scannedAssetsCounts.Add(typeName, 1);
                    }
                    else
                    {
                        scannedAssetsCounts[typeName]++;
                    }
                }
            }

            foreach (AssetEntity item in assetsInRoom)
            {

                string typeName = item.type.name;
                if (!roomAssetsCounts.ContainsKey(typeName))
                {
                    roomAssetsCounts.Add(typeName, 1);
                }
                else
                {
                    roomAssetsCounts[typeName]++;
                }

            }

            foreach (ReportPositionEntity item in reportPositionEntities)
            {
                string typeName = item.asset.type.name;
                if (scannedAssetsCounts.ContainsKey(typeName) && roomAssetsCounts.ContainsKey(typeName) && !result.ContainsKey(typeName))
                {
                    result.Add(typeName, scannedAssetsCounts[typeName] + "/" + roomAssetsCounts[typeName]);
                }               
            }

            string listedAssets = "";
            bool first = true;
            foreach (KeyValuePair<string, string> item in result)
            {
                if (first)
                {
                    listedAssets = item.Key + "    " + item.Value;
                    first = false;
                }
                listedAssets += Environment.NewLine + item.Key + "    " + item.Value;
            }

            return listedAssets;
        }

        private Dictionary<string, string> Get(ReportPositionEntity[] reportPositionEntities, AssetEntity[] assetsInRoom)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Dictionary<string, int> scannedAssetsCounts = new Dictionary<string, int>();
            Dictionary<string, int> roomAssetsCounts = new Dictionary<string, int>();

            foreach (ReportPositionEntity item in reportPositionEntities)
            {

                string typeName = item.asset.type.name;
                if (!scannedAssetsCounts.ContainsKey(typeName))
                {
                    scannedAssetsCounts.Add(typeName, 1);
                }
                else
                {
                    scannedAssetsCounts[typeName]++;
                }

            }

            foreach (AssetEntity item in assetsInRoom)
            {

                string typeName = item.type.name;
                if (!roomAssetsCounts.ContainsKey(typeName))
                {
                    roomAssetsCounts.Add(typeName, 1);
                }
                else
                {
                    roomAssetsCounts[typeName]++;
                }

            }

            foreach (ReportPositionEntity item in reportPositionEntities)
            {
                string typeName = item.asset.type.name;
                if (scannedAssetsCounts.ContainsKey(typeName) && roomAssetsCounts.ContainsKey(typeName) && !result.ContainsKey(typeName))
                {
                    result.Add(typeName, scannedAssetsCounts[typeName] + "/" + roomAssetsCounts[typeName]);
                }
            }

            return result;
        }
    }
}