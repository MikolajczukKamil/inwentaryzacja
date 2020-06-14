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
    public partial class ReportButton : ContentView
    {
        public static readonly BindableProperty ButtonTextProperty1 =
           BindableProperty.Create("Rtitle", typeof(string), typeof(ReportButton), default(string));
        public string Rtitle
        {
            get { return (string)GetValue(ButtonTextProperty1); }
            set { SetValue(ButtonTextProperty1, value); }
        }
        public static readonly BindableProperty ButtonTextProperty2 =
          BindableProperty.Create("RData", typeof(string), typeof(ReportButton), default(string));
        public string RData
        {
            get { return (string)GetValue(ButtonTextProperty2); }
            set { SetValue(ButtonTextProperty2, value); }
        }
        public static readonly BindableProperty ButtonTextProperty3 =
        BindableProperty.Create("RHall", typeof(string), typeof(ReportButton), default(string));
        public string RHall
        {
            get { return (string)GetValue(ButtonTextProperty3); }
            set { SetValue(ButtonTextProperty3, value); }
        }
        public ReportButton()
        {
            InitializeComponent();
           Reporttitle.SetBinding(Label.TextProperty, new Binding("Rtitle", source: this));
           Reportdata.SetBinding(Label.TextProperty, new Binding("RData", source: this));
           ReportHall.SetBinding(Label.TextProperty, new Binding("RHall", source: this));
        }
        
    }
}