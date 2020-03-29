using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kalkulator2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if(Editor1.Text != "" && Editor2.Text !="")
            {
                double dodawanie = Convert.ToDouble(Editor1.Text) + Convert.ToDouble(Editor2.Text);
                double odejmowanie = Convert.ToDouble(Editor1.Text) - Convert.ToDouble(Editor2.Text);
                double mnoezenie = Convert.ToDouble(Editor1.Text) * Convert.ToDouble(Editor2.Text);
                Label1.Text = "Dodawanie: " + dodawanie + "\nOdejmowanie: " + odejmowanie + "\nMnozenie: " + mnoezenie + "\nDzielenie: ";
                if (Editor2.Text == "0")
                    Label1.Text += "Dzielenie przez 0";
                else
                {
                    double dzielenie = Convert.ToDouble(Editor1.Text) / Convert.ToDouble(Editor2.Text);
                    Label1.Text += dzielenie;
                }
            }
        }
    }
}
