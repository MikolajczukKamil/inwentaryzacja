using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_allReports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportDetailsView : ContentPage
    {
        string ReportName;
        
        public ReportDetailsView(string reportName)
        {
            InitializeComponent();
            BindingContext = this;
            ReportHeader.Text = reportName;
        }
    }
}