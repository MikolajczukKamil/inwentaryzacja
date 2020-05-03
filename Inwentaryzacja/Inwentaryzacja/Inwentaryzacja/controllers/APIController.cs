using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private string url = "https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI";


        private async Task<string> sendRequest(string address)
        {
            string result;
            int statusCode = 200;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri(url + address);
                    var response = await App.clientHttp.GetAsync(uri);
                    result = await response.Content.ReadAsStringAsync();
                    statusCode = (int)response.StatusCode;

                    if(response.IsSuccessStatusCode)
                    {
                        return result;
                    }
                }
                catch (Exception failConnection)
                {
                    
                    result = "{\"message\":\"" + failConnection.Message + "\"}";
                    statusCode = 502;
                }
            }
            else
            {
                result = "{\"message\":\"No internet connection\"}";
                statusCode = 402;
            }

            ErrorInvoke(result, statusCode);
            return null;
        }
        private async Task<bool> sendRequest(string address, StringContent cont)
        {
            string result;
            int statusCode = 200;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri(url + address);
                    var response = await App.clientHttp.PostAsync(uri, cont);
                    result = await response.Content.ReadAsStringAsync();
                    statusCode = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception failConnection)
                {
                    result = "{\"message\":\"" + failConnection.Message + "\"}";
                    statusCode = 502;
                }
            }
            else
            {
                result = "{\"message\":\"No internet connection\"}";
                statusCode = 402;
            }

            ErrorInvoke(result, statusCode);
            return false;
        }
        private async Task<bool> authorizationRequest(string address, StringContent cont)
        {
            string result;
            int statusCode = 200;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri(url + address);
                    var response = await App.clientHttp.PostAsync(uri, cont);
                    result = await response.Content.ReadAsStringAsync();
                    statusCode = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        var header = response.Headers.GetValues("Authorization").First().ToString();
                        header = header.Remove(0, 7);
                        App.clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

                        return true;
                    }
                }
                catch (Exception failConnection)
                {
                    result = "{\"message\":\"" + failConnection.Message + "\"}";
                    statusCode = 502;
                }
            }
            else
            {
                result = "{\"message\":\"No internet connection\"}";
                statusCode = 402;
            }

            ErrorInvoke(result, statusCode);
            return false;
        }

        private T convertJSONToObject<T>(string json)
        {
            T entity;
            try
            {
                entity = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                entity = default;
                ErrorInvoke("{\"message\":\"Cannot convert data.\"}", 404);
            }

            return entity;
        }

        private string convertDataToJSON<T>(T data)
        {
            string entity;
            try
            {
                entity = JsonConvert.SerializeObject(data);
            }
            catch (Exception)
            {
                entity = default;
                ErrorInvoke("{\"message\":\"Cannot convert data.\"}", 404);
            }

            return entity;
        }

        private void ErrorInvoke(string statement, int statusCode)
        {
            if (ErrorEventHandler != null)
            {
                ErrorEventArgs e;
                try
                {
                    e = JsonConvert.DeserializeObject<ErrorEventArgs>(statement);
                }
                catch(Exception failConnection)
                {
                    e = new ErrorEventArgs
                    {
                        message = statement
                    };
                }
                e.SetErrorStatus(statusCode);
                ErrorEventHandler(this, e);
            }
        }

        public event EventHandler<ErrorEventArgs> ErrorEventHandler;


        //Asset
        public async Task<AssetEntity> getAssetByID(int id)
        {
            var uri = "/asset/read_one.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<AssetEntity>(content);

            return entity;
        }

        public async Task<List<AssetEntity>> getAllAssets()
        {
            var uri = "/asset/read.php";
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<AssetEntity>>(content);

            return entity;
        }

        public async Task<bool> sendAsset(string name, int asset_type)
        {
            string data = "{\"name\":\"" + name + "\", \"asset_type\":\"" + asset_type + "\"}";
            var uri = "/asset/create.php";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        public async Task<bool> deleteAssetByID(int id)
        {
            var uri = "/asset/delete.php";
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        //AssetType
        public async Task<AssetTypeEntity> getAssetTypeByID(int id)
        {
            var uri = "/asset_type/read_one.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<AssetTypeEntity>(content);

            return entity;
        }

        public async Task<List<AssetTypeEntity>> getAllAssetTypes()
        {
            var uri = "/asset_type/read.php";
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<AssetTypeEntity>>(content);

            return entity;
        }

        public async Task<bool> sendAssetType(string name , string letter)
        {
            var uri = "/asset_type/create.php";
            string data = "{\"name\":\"" + name + "\", \"letter\":\"" + letter + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        public async Task<bool> deleteAssetTypeByID(int id)
        {
            var uri = "/asset_type/delete.php";
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);
           
            return content;
        }

        //Building
        public async Task<BuildingEntity> getBuildingByID(int id)
        {
            var uri = "/building/read_one.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<BuildingEntity>(content);

            return entity;
        }

        public async Task<List<BuildingEntity>> getAllBuildings()
        {
            var uri = "/building/read.php";
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<BuildingEntity>>(content);

            return entity;
        }

        public async Task<bool> sendBuilding(string name)
        {
            var uri = "/building/create.php";
            string data = "{\"name\":\"" + name + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);
            
            return content;
        }

        public async Task<bool> deleteBuildingByID(int id)
        {
            var uri = "/building/delete.php";
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        //Report
        public async Task<ReportEntity> getReportByID(int id)
        {
            var uri = "/report/read_one.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<ReportEntity>(content);

            return entity;
        }

        public async Task<List<ReportEntity>> getAllReports()
        {
            var uri = "/report/read.php";
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<ReportEntity>>(content);

            return entity;
        }

        public async Task<bool> sendReportWithAssets(string name, int room, int[] assets)
        {
            var uri = "/report/create.php";
            string data = convertDataToJSON<ReportWithAssetEntity>(new ReportWithAssetEntity(name, room, assets));
            if (data == null) return false;
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);
          
            return content;
        }

        public async Task<bool> deleteReportByID(int id)
        {
            var uri = "/report/delete.php";
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);
    
            return content;
        }

        //Room
        public async Task<RoomEntity> getRoomByID(int id)
        {
            var uri = "/room/read_one.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<RoomEntity>(content);

            return entity;
        }

        public async Task<List<RoomEntity>> getAllRooms()
        {
            var uri ="/room/read.php";
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<RoomEntity>>(content);

            return entity;
        }

        public async Task<bool> sendRoom(string name, int building)
        {
            var uri = "/room/create.php";
            string data = "{\"name\":\"" + name + "\", \"building\":\"" + building + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        public async Task<bool> deleteRoomByID(int id)
        {        
            var uri = "/room/delete.php";
            var data = "{\"id\":\"" + id + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        //User
        public async Task<bool> createUser(string login, string password)
        {
            //prototypowa metoda, tworzy tylko usera testowego
            var uri = "/creator/user_creator.php";
            var data = "{\"login\":\"" + login + "\", \"password\":\"" + password + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        public async Task<bool> loginUser(string login, string password)
        {
            var uri = "/login/login.php";
            var data = "{\"login\":\"" + login + "\", \"password\":\"" + password + "\"}";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await authorizationRequest(uri, cont);

            return content;
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public int errorStatus { get; set; }
        public string message { get; set; }
        public string messageForUser { get; set; }

        public bool auth = true;

        public void SetErrorStatus(int code)
        {
            errorStatus = code;
            switch (code)
            {
                case 400:
                    if (auth)
                        messageForUser = "Niekompletne dane, nie można wykonać zapytania.";
                    else
                        messageForUser = "Błąd autoryzacji, wymagane ponowne logowanie.";
                    break;
                case 401:
                    messageForUser = "Błędny login lub hasło, proszę spróbować jeszcze raz."; break;
                case 402:
                    messageForUser = "Brak połączenia z Internetem, sprawdź swoje połączenie."; break;
                case 404:
                    messageForUser = "Nie można odczytać danych."; break;
                case 500:
                    messageForUser = "Błąd przy tworzeniu sesji, proszę spróbować jeszcze raz."; break;
                case 502:
                    messageForUser = "Nie odnaleziono serwera. Sprawdź połączenie z internetem, bądź zmień połączenie sieciowe."; break;
                case 503:
                    if (auth)
                        messageForUser = "Nie udało się edytować danych. Usługa czasowo niedostępna.";
                    else
                        messageForUser = "Błąd autoryzacji, wymagane ponowne logowanie.";
                    break;
                default:
                    messageForUser = "Niezidentyfikowany błąd."; break;
            }
        }
    }
}
