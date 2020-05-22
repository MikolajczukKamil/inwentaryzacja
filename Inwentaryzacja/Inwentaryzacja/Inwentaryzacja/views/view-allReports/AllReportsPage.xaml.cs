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
            api = new APIController();
            List<AllReport> rp = new List<AllReport>();
            //ReportHeaderEntity[] rep = api.getReportHeaders().Result;
            //for (int i = 0; i < rep.Length; i++)
            //{
            //    rp.Add(new AllReport(){ReportName = rep[i].name, ReportRoom= rep[i].room.name, ReportDate= Convert.ToString(rep[i].create_date) });
            //}
            rp.Add(new AllReport() {ReportName = "Report#1", ReportRoom = "77A", ReportDate = Convert.ToString(new DateTime(10000000))});
            rp.Add(new AllReport() {ReportName = "Report#2", ReportRoom = "22C", ReportDate = Convert.ToString(new DateTime(10000000)) });

            InitializeComponent();
            ReportList.ItemsSource = rp;
            
        }
        
        private async void back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new WelcomeViewPage());
        }
        public class AllReport
        {
            public string ReportName { get; set; }
            public string ReportRoom { get; set; }
            public string ReportDate { get; set; }
        }
    }
}