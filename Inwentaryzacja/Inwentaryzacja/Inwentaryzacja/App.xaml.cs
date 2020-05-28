using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Inwentaryzacja
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var session = new SessionController(new APIController());
            if(session.ResumeSession())
            {
                MainPage = new NavigationPage(new WelcomeViewPage());
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {

        }
    }
}
