using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cal
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}


		private void Button_OnClicked(object sender, EventArgs e)
		{
			Calculate();
		}

		private void Calculate()
		{
			double a = Convert.ToDouble(Input_a.Text);
			double b = Convert.ToDouble(Input_b.Text);
			double addition = a + b;
			double substraction = a - b;
			double multiplication = a * b;
			double division = a / b;

			Label_Addition.Text = Convert.ToString(addition);
			Label_Substraction.Text = Convert.ToString(substraction);
			Label_Multiplication.Text = Convert.ToString(multiplication);
			Label_Division.Text = Convert.ToString(division);
		}
	}
}
