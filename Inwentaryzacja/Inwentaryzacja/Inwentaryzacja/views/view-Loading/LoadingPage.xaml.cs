using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inwentaryzacja.views.view_Loading
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    /// <summary>
    /// Klasa odpowiadajaca za widok okna ladowania
    /// </summary>
    public partial class LoadingPage : ContentPage
    {
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        public LoadingPage()
        {
            InitializeComponent();
        }
    }
}