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
        double firstNumber;
        double secondNumber;

        public MainPage()
        {
            InitializeComponent();
            FirstNumber.TextChanged += (o, e) => firstNumber = double.Parse(FirstNumber.Text);
            SecondNumber.TextChanged += (o, e) => secondNumber = double.Parse(SecondNumber.Text);
            
            Calculate.Pressed += (o, e) =>
            {
                Mul.Text = "Multiplication: " + firstNumber * secondNumber;
                if (secondNumber != 0) Div.Text = "Division: " + firstNumber / secondNumber;
                else Div.Text = "Division: cannot divide by 0!";
                Add.Text = "Addition: " + (firstNumber + secondNumber);
                Sub.Text = "Subtraction: " + (firstNumber - secondNumber);                
            };
        }

    }
}
