using Xamarin.Forms;

namespace Inwentaryzacja
{
	/// <summary>
	/// Wybornik bez ramki
	/// </summary>
	public class BorderlessPicker : Picker
	{
		/// <summary>
		/// Konstruktor wybornika bez ramki
		/// </summary>
		public BorderlessPicker() : base()
		{
			
		}

		/// <summary>
		/// Przypisuje tymczasowa wartosc
		/// </summary>
		public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(  
			propertyName: nameof(Placeholder),  
			returnType: typeof(string),  
			declaringType: typeof(string),  
			defaultValue: string.Empty);  
  
		/// <summary>
		/// Tymczasowa wartosc
		/// </summary>
		public string Placeholder  
		{  
			get { return (string)GetValue(PlaceholderProperty); }  
			set { SetValue(PlaceholderProperty, value); }  
		}  
	}
}
