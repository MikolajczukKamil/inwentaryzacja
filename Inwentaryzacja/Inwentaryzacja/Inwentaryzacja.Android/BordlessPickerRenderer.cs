using System.ComponentModel;
using Android.Content;
using Inwentaryzacja;
using Inwentaryzacja.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BordlessPickerRenderer))]

namespace Inwentaryzacja.Droid
{
	public class BordlessPickerRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
	{
		private BorderlessPicker picker = null;
		public BordlessPickerRenderer(Context context) : base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)  
		{  
			base.OnElementChanged(e);  
			if (e.NewElement != null)  
			{  
				picker = Element as BorderlessPicker;  
				UpdatePickerPlaceholder();  
				if (picker.SelectedIndex <= -1)  
				{  
					UpdatePickerPlaceholder();  
				}  
			}  
		}  
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)  
		{  
			base.OnElementPropertyChanged(sender, e);  
			if (picker != null)
			{
				Control.Background = null;

				if (e.PropertyName.Equals(BorderlessPicker.PlaceholderProperty.PropertyName))  
				{  
					UpdatePickerPlaceholder();  
				}  
			}  
		}  
  
		protected override void UpdatePlaceHolderText()  
		{  
			UpdatePickerPlaceholder();  
		}  
  
		void UpdatePickerPlaceholder()  
		{  
			if (picker == null)  
				picker = Element as BorderlessPicker;  
			if (picker.Placeholder != null)  
				Control.Hint = picker.Placeholder;  
		}
	}
}

