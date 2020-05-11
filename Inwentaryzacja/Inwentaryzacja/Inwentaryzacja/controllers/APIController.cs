using Inwentaryzacja.models;
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
    public class APIController
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
                catch(Exception)
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
        public async Task<AssetInfoEntity> getAssetInfoByID(int id)
        {
            var uri = "/asset/getAssetInfo.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<AssetInfoEntity>(content);

            return entity;
        }

        public async Task<bool> addNewAsset(AssetPrototype asset)
        {
            string data = convertDataToJSON<AssetPrototype>(asset);
            if (String.IsNullOrEmpty(data)) return false;
            var uri = "/asset/addNewAsset.php";
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

        public async Task<bool> createAssetType(AssetTypePrototype assetType)
        {
            string data = convertDataToJSON<AssetTypePrototype>(assetType);
            if (String.IsNullOrEmpty(data)) return false;
            var uri = "/asset_type/create.php";
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

        public async Task<List<BuildingEntity>> getBuildings()
        {
            var uri = "/building/getBuildings.php";
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<BuildingEntity>>(content);

            return entity;
        }

        public async Task<bool> addNewBuilding(BuildingPrototype building)
        {
            string data = convertDataToJSON<BuildingPrototype>(building);
            if (String.IsNullOrEmpty(data)) return false;
            var uri = "/building/addNewBuilding.php";
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

        public async Task<List<RoomEntity>> getRoomsByBuildingID(int id)
        {
            var uri = "/building/getRooms.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<RoomEntity>>(content);

            return entity;
        }


        //Room
        public async Task<bool> addNewRoom(RoomPropotype room)
        {
            string data = convertDataToJSON<RoomPropotype>(room);
            if (String.IsNullOrEmpty(data)) return false;
            var uri = "/room/addNewRoom.php";
            var cont = new StringContent(data, Encoding.UTF8, "application/json");
            var content = await sendRequest(uri, cont);

            return content;
        }

        public async Task<List<AssetInRoom>> getAssetsInRoomByID(int id)
        {
            var uri = "/room/getAssetsInRoom.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<AssetInRoom>>(content);

            return entity;
        }


        //Report
        public async Task<ReportHeaderEntity> getReportHeaderByID(int id)
        {
            var uri = "/report/getReportHeader.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<ReportHeaderEntity>(content);

            return entity;
        }

        public async Task<List<ReportHeaderEntity>> getReportsHeaders()
        {
            var uri = "/report/getReportsHeaders.php";
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<ReportHeaderEntity>>(content);

            return entity;
        }

        public async Task<bool> addNewReport(ReportPrototype report)
        {
            var uri = "/report/addNewReport.php";
            string data = convertDataToJSON<ReportPrototype>(report);
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

        public async Task<List<AssetInRoom>> getAssetsInReportByID(int id)
        {
            var uri = "/report/getAssetsInReport.php?id=" + id.ToString();
            var content = await sendRequest(uri);
            var entity = convertJSONToObject<List<AssetInRoom>>(content);

            return entity;
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
            var uri = "/login/addLoginSession.php";
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
