using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Inwentaryzacja
{
    /// <summary>
    /// Niestandardowa komorka widoku
    /// </summary>
    public class CustomViewCell : ViewCell
    {
        /// <summary>
        /// Kolor komorki
        /// </summary>
        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create("SelectedBackgroundColor",
                                    typeof(Color),
                                    typeof(CustomViewCell),
                                    Color.Default);

        /// <summary>
        /// Wybierz kolor komorki
        /// </summary>
        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }
    }
}
