using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        public double numer;
        public double numerr;
        
        public MainPage()
        {
            InitializeComponent();
            
        }
  
        private void Button_Clicked(object sender, EventArgs e)
        {
          
           dod.Text = Convert.ToString(operacje.Dodaj(numer, numerr));
           od.Text = Convert.ToString(operacje.odejmij(numer, numerr));
           mn.Text = Convert.ToString(operacje.Pomnoz(numer, numerr));
           dz.Text = Convert.ToString(operacje.Podziel(numer, numerr));
          
        }
        void EditorCompleted1(object sender, EventArgs e)
        {
            var text = ((Editor)sender).Text;
            
            numer = Convert.ToDouble(text);
        }
        void EditorCompleted2(object sender, EventArgs e)
        {
            var text = ((Editor)sender).Text;
            
            numerr = Convert.ToDouble(text);
        }
       

    }
}
