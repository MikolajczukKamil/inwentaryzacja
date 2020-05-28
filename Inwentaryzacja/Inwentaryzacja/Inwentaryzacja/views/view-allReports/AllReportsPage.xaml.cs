using Inwentaryzacja.Controllers.Api;
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

        private void View_report_clicked(object sender, EventArgs e)
        {
            AllReport selected = (AllReport)ReportList.SelectedItem;

            DisplayAlert("a",selected.ReportName, "A");
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
        }
    }
}