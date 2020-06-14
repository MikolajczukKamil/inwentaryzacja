using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Inwentaryzacja
{

    /// <summary>
    /// Klasa startowa inicjalizujaca cala aplikacje, generowana automatycznie
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }
        /// <summary>
        /// Funkcja wywolywana przy starcie aplikacji
        /// </summary>
        protected override void OnStart()
        {
            // Handle when your app starts
        }
        /// <summary>
        /// Funkcja wywolywana przy przejsciu aplikacji w stan uspienia
        /// </summary>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }
        /// <summary>
        /// Funkcja wywolywana przy powrocie do aplikacji
        /// </summary>
        protected override void OnResume()
        {

        }
    }
}
