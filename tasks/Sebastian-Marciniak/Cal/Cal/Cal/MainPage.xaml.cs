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

		private int count;
		private void Button_OnClicked(object sender, EventArgs e)
		{
			count++;
			((Button) sender).Text = $"You clicked {count} times.";
		}
	}
}
