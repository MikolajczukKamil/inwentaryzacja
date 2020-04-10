using Android.Content;
using Inwentaryzacja;
using Inwentaryzacja.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BordlessPickerRenderer))]

namespace Inwentaryzacja.Droid
{
	public class BordlessPickerRenderer : PickerRenderer
	{
		public BordlessPickerRenderer(Context context) : base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				Control.Background = null;
			}
		}

	}
}

