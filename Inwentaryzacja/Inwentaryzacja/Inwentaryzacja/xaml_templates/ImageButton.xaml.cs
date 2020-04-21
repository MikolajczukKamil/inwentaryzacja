using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageButton : ContentView
    {
        public static readonly BindableProperty ButtonTextProperty =
            BindableProperty.Create("ButtonText", typeof(string), typeof(ImageButton), default(string));
        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }
        public static readonly BindableProperty ImageSourceProperty =
           BindableProperty.Create("Source", typeof(ImageSource), typeof(ImageButton), default(ImageSource));
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public ImageButton()
        {
            InitializeComponent();
            innerLabel.SetBinding(Label.TextProperty, new Binding("ButtonText", source: this));
            innerImage.SetBinding(Image.SourceProperty, new Binding("Source", source: this));
        }
    }
}