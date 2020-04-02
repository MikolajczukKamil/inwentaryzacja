using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace kalkulator
{
    /// <summary>
    ///     Program odwołuje się do elementów wyświetlanych na ekranie i na ich podstawie dokonuje obliczeń
    /// </summary>

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        /// <summary>
        ///     Funkcja ObliczTo wywoływana jest po naciśnięciu jedynego przycisku.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ObliczTo(object sender, System.EventArgs e)
        {
            double a,b;
            if(Double.TryParse(liczba1.Text,out a) && Double.TryParse(liczba2.Text,out b))
            {
                dodawanie.Text = "Dodawanie: " + Convert.ToString(a + b);
                odejmowanie.Text = "Odejmowanie: " + Convert.ToString(a - b);
                mnozenie.Text = "Mnożenie: " + Convert.ToString(a * b);
                if(b!=0)
                {
                    dzielenie.Text = "Dzielenie: " + Convert.ToString(a / b);
                }
                else
                {
                    dzielenie.Text = "Dzielenie: Złe dane!";
                }
                
            }
            else
            {
                dodawanie.Text = "Dodawanie: Złe dane!";
                odejmowanie.Text = "Odejmowanie: Złe dane!";
                mnozenie.Text = "Mnożenie Złe dane!";
                dzielenie.Text = "Dzielenie: Złe dane!";
            }
        }


        public MainPage()
        {
            InitializeComponent();
        }
    }
}
