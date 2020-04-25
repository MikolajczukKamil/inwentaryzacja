using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Inwentaryzacja.controllers
{
    public class APIController
    {
        private static async Task<string> sendRequest(Uri uri)
        {
            string result;
            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var response = await App.clientHttp.GetAsync(uri);
                    result = await response.Content.ReadAsStringAsync();
                }
                catch (Exception failConnection)
                {
                    result = "{\"message\":\"" + failConnection.Message + "\"}";
                }
            }
            else
            {
                result = "{\"message\":\"No internet connection\"}";
            }

            return result;
        }
        private static async Task<string> sendRequest(Uri uri, StringContent cont)
        {
            string result;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var response = await App.clientHttp.PostAsync(uri, cont);
                    result = await response.Content.ReadAsStringAsync();
                }
                catch (Exception failConnection)
                {
                    result = "{\"message\":\"" + failConnection.Message + "\"}";
                }
            }
            else
            {
                result = "{\"message\":\"No internet connection\"}";
            } 

            return result;
        }


        //Asset
        public static async Task<string> takeAssetByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset/read_one.php?id=" + id.ToString());
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> takeAllAssets()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset/read.php");
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> sendAsset(string name, int asset_type)
        {
            string data = "{\"name\":\"" + name + "\", \"asset_type\":\"" + asset_type + "\"}";
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset/create.php");
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);

            return content;
        }

        public static async Task<string> deleteAssetByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);

            return content;
        }

        //AssetType
        public static async Task<string> takeAssetTypeByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset_type/read_one.php?id=" + id.ToString());
            string content = await APIController.sendRequest(uri);
      
            return content;
        }

        public static async Task<string> takeAllAssetTypes()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset_type/read.php");
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> sendAssetType(string name , string letter)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset_type/create.php");
            string data = "{\"name\":\"" + name + "\", \"letter\":\"" + letter + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);

            return content;
        }

        public static async Task<string> deleteAssetTypeByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset_type/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);
           
            return content;
        }

        //Building
        public static async Task<string> takeBuildingByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/building/read_one.php?id=" + id.ToString());
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> takeAllBuildings()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/building/read.php");
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> sendBuilding(string name)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/building/create.php");
            string data = "{\"name\":\"" + name + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);
            
            return content;
        }

        public static async Task<string> deleteBuildingByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/building/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);

            return content;
        }

        //Report
        public static async Task<string> takeReportByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/report/read_one.php?id=" + id.ToString());
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> takeAllReports()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/report/read.php");
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> sendReport(string name, int room, DateTime create_date, int owner)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/report/create.php");
            string data = "{\"name\":\"" + name + "\", \"room\":\"" + room + "\", \"create_date\":\"" + create_date + "\", \"owner\":\"" + owner + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);
          
            return content;
        }

        public static async Task<string> deleteReportByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/report/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);
    
            return content;
        }

        //Room
        public static async Task<string> takeRoomByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/room/read_one.php?id=" + id.ToString());
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> takeAllRooms()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/room/read.php");
            string content = await APIController.sendRequest(uri);

            return content;
        }

        public static async Task<string> sendRoom(string name, int building)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/room/create.php");
            string data = "{\"name\":\"" + name + "\", \"building\":\"" + building + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);

            return content;
        }

        public static async Task<string> deleteRoomByID(int id)
        {        
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/room/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);

            return content;
        }

        //User
        public static async Task<string> createUser(string login, string password)
        {
            //prototypowa metoda, tworzy tylko usera testowego
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/creator/");
            var data = "{\"login\":\"" + login + "\", \"password\":\"" + password + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            string content = await APIController.sendRequest(uri, cont);

            return content;
        }
    }
}
