using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kalkulator
{


    public partial class MainPage : ContentPage
    {
        //Dwie pobrane cyfry
        float number1 = 0;
        float number2 = 0;

        public MainPage()
        {
            InitializeComponent();

            //Dodanie eventu kliknięcia w button
            result.Clicked += (o, e) =>
            {
                //Gdy liczba nie zostanie wpisania, wyświetl alert
                if (numberOne.Text == "" || numberTwo.Text == "" || numberOne.Text == null || numberTwo.Text == null)
                {
                    DisplayAlert("Warning!", "You must enter the numbers!", "OK");
                    return;
                }

                //Zainicjowanie pól wartościami wpisanymi w pola Editor
                number1 = float.Parse(numberOne.Text);
                number2 = float.Parse(numberTwo.Text);

                //Podmiana tekstu w znacznikach Label 
                mul.Text = "Multiplication: " + (number1 * number2);
                sub.Text = "Subtraction: " + (number1 - number2);
                if (number2 != 0) div.Text = "Division: " + (number1 / number2);
                else div.Text = "Division: Cannot divide by 0!";
                add.Text = "Addition: " + (number1 + number2);
            };
        }
    }
}

