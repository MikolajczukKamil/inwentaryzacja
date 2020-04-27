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
    class APIController
    {
        private async Task<(string,bool)> sendRequest(Uri uri)
        {
            string result;
            bool done = false;
            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var response = await App.clientHttp.GetAsync(uri);
                    result = await response.Content.ReadAsStringAsync();
                    if(response.IsSuccessStatusCode)
                    {
                        done = true;
                    }
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

            return (result,done);
        }
        private async Task<(string, bool)> sendRequest(Uri uri, StringContent cont)
        {
            string result;
            bool done = false;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var response = await App.clientHttp.PostAsync(uri, cont);
                    result = await response.Content.ReadAsStringAsync();
                    if(response.IsSuccessStatusCode)
                    {
                        done = true;
                    }
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

            return (result,done);
        }


        //Asset
        public async Task<(string, bool)> getAssetByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset/read_one.php?id=" + id.ToString());
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> getAllAssets()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset/read.php");
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> sendAsset(string name, int asset_type)
        {
            string data = "{\"name\":\"" + name + "\", \"asset_type\":\"" + asset_type + "\"}";
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset/create.php");
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        public async Task<(string, bool)> deleteAssetByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        //AssetType
        public async Task<(string, bool)> getAssetTypeByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset_type/read_one.php?id=" + id.ToString());
            var content = await sendRequest(uri);
      
            return content;
        }

        public async Task<(string, bool)> getAllAssetTypes()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset_type/read.php");
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> sendAssetType(string name , string letter)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset_type/create.php");
            string data = "{\"name\":\"" + name + "\", \"letter\":\"" + letter + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        public async Task<(string, bool)> deleteAssetTypeByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/asset_type/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);
           
            return content;
        }

        //Building
        public async Task<(string, bool)> getBuildingByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/building/read_one.php?id=" + id.ToString());
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> getAllBuildings()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/building/read.php");
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> sendBuilding(string name)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/building/create.php");
            string data = "{\"name\":\"" + name + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);
            
            return content;
        }

        public async Task<(string, bool)> deleteBuildingByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/building/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        //Report
        public async Task<(string, bool)> getReportByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/report/read_one.php?id=" + id.ToString());
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> getAllReports()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/report/read.php");
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> sendReport(string name, int room, DateTime create_date, int owner)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/report/create.php");
            string data = "{\"name\":\"" + name + "\", \"room\":\"" + room + "\", \"create_date\":\"" + create_date + "\", \"owner\":\"" + owner + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);
          
            return content;
        }

        public async Task<(string, bool)> deleteReportByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/report/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);
    
            return content;
        }

        //Room
        public async Task<(string, bool)> getRoomByID(int id)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/room/read_one.php?id=" + id.ToString());
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> getAllRooms()
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/room/read.php");
            var content = await sendRequest(uri);

            return content;
        }

        public async Task<(string, bool)> sendRoom(string name, int building)
        {
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/room/create.php");
            string data = "{\"name\":\"" + name + "\", \"building\":\"" + building + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        public async Task<(string, bool)> deleteRoomByID(int id)
        {        
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/room/delete.php");
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        //User
        public async Task<(string, bool)> createUser(string login, string password)
        {
            //prototypowa metoda, tworzy tylko usera testowego
            var uri = new Uri("https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI/creator/");
            var data = "{\"login\":\"" + login + "\", \"password\":\"" + password + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }
    }
}
