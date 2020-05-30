using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using Inwentaryzacja.Services;
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
            await DisplayAlert("Błąd", error.MessageForUser, "OK");
        }

        public class AllReport
        {
            public string ReportName { get; set; }
            public string ReportRoom { get; set; }
            public string ReportDate { get; set; }
        }

        private void back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
        }

        private async void ReportList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            EnableView(false);
            ReportService reportService = new ReportService(api);

            AllReport selectedReport = (AllReport)ReportList.SelectedItem;

            ReportHeaderEntity reportHeaderEntity = null;

            if (selectedReport == null) return;
            foreach (ReportHeaderEntity item in reportHeaders)
            {
                if (item.name == selectedReport.ReportName)
                {
                    reportHeaderEntity = item;
                }
            }
            if (reportHeaderEntity == null) return;

            ReportPositionEntity[] reportPositionEntities = await reportService.GetReportPositions(reportHeaderEntity.id);
            if (reportPositionEntities == null) return;

            string[] counted = reportService.GetScannedItemsCount(reportPositionEntities, reportHeaderEntity.room);

            string inThisRoom = counted[0];
            string moveToRoom = counted[1];
            string moveFromRoom = counted[2];
            string inAnotherRoom = counted[3];
            string scannedAll = counted[4];

            string scannedAllDetails = counted[5];
            string inThisRoomDetails = counted[6];
            string movedToRoomDetails = counted[7];
            string movedFromRoomDetails = counted[8];
            string inAnotherRoomDetails = counted[9];

            string headerText = reportHeaderEntity.name;
            string roomText = reportHeaderEntity.room.name;
            DateTime date = reportHeaderEntity.create_date;
            string editedDay = date.Day < 10 ? "0" + date.Day : "" + date.Day;
            string editedMonth = date.Month < 10 ? "0" + date.Month : "" + date.Month;
            string createDate = editedDay + "." + editedMonth + "." + date.Year + "r.";
            string createTime = date.TimeOfDay.ToString();
            string ownerText = reportHeaderEntity.owner.login;

            EnableView(true);
            App.Current.MainPage = new ReportDetailsView(headerText, roomText, createDate, createTime, ownerText, inThisRoom, moveToRoom, moveFromRoom, inAnotherRoom, scannedAll, scannedAllDetails, inThisRoomDetails, movedToRoomDetails, movedFromRoomDetails, inAnotherRoomDetails);
        }

        private void EnableView(bool state)
        {
            IsBusy = !state;
            ReportList.IsEnabled = state;
        }
    }
}