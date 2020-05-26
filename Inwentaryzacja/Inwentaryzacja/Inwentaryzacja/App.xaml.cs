using Inwentaryzacja.Controllers.Api;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Inwentaryzacja.views.view_Loading;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Inwentaryzacja
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new ChooseRoomPage();
        }

        protected async override void OnStart()
        {
            APIController api = new APIController();
            await api.LoginUser("user1", "111");
            MainPage = new ChooseRoomPage();
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
