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
        APIController api;
        public AllReportsPage()
        {
            InitializeComponent();
        }
        private async void back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            api = new APIController();
            List<AllReport> rp = new List<AllReport>();
            Task<ReportHeaderEntity[]> header_task = api.getReportHeaders();
            await header_task;
            ReportHeaderEntity[] rep = header_task.Result;
            for (int i = 0; i < rep.Length; i++)
            {
                rp.Add(new AllReport() { ReportName = rep[i].name, ReportRoom = rep[i].room.name, ReportDate = Convert.ToString(rep[i].create_date) });
            }
            ReportList.ItemsSource = rp;
        }
        public class AllReport
        {
            public string ReportName { get; set; }
            public string ReportRoom { get; set; }
            public string ReportDate { get; set; }
        }
    }
}