using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void Oblicz(object sender,EventArgs e)
        {
            //sprawdzanie zawartości
            if (Liczba1.Text=="" || Liczba2.Text=="" || Liczba1.Text == null || Liczba2.Text == null)
            {
                DisplayAlert("Uwaga!", "Wpisz prawidłowo Liczby", "OK");
                return;
            }
            else
            { 
            //Pobranie wartości
            double a = Convert.ToDouble(Liczba1.Text);
            double b = Convert.ToDouble(Liczba2.Text);
                Add.Text ="Suma: "+(a + b);
                Sub.Text ="Różnica: "+ (a - b);
                Mull.Text ="Iloczyn: "+(a * b);
                if (b == 0)
                {
                    DisplayAlert("ZEROO!!!!", "Nie dziel przez 0", "OK");
                    return;
                }
                else {Div.Text ="Iloraz: "+ (a / b); }
                
            }
    }
    }
}
