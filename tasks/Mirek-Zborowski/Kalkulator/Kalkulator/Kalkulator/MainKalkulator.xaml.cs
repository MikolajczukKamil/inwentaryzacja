using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Kalkulator
{
	
	public partial class MainKalkulator : ContentPage
	{
		public MainKalkulator()
		{
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e) //Обработчик нажатия на кнопку
		{
			try
			{
				int n1 = Convert.ToInt32(num1.Text); //Конвертация значений из текстовых полей в числа
				int n2 = Convert.ToInt32(num2.Text);

				result.Text = (n1 + n2).ToString(); //Сложение полученных чисел, преобразование их обратно в строку и вывод в один из элементов <Label>
			}
			catch (Exception exc) { }
		}
	}
}