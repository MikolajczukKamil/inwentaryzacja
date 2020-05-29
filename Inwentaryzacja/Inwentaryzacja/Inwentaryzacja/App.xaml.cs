using Inwentaryzacja.controllers.session;
using Inwentaryzacja.Controllers.Api;
using System.Threading.Tasks;
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

            var api = new APIController();
            var session = new SessionController(api);
            if(session.ResumeSession() && Task.Run(() => api.getAssetInfo(1)).Result != null)
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
