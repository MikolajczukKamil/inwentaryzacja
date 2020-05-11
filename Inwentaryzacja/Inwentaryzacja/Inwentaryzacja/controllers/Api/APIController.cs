using Inwentaryzacja.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Inwentaryzacja.Controllers.Api
{
    public class APIController
    {
        private string BaseUrl = "https://aplikacja-do-inwentaryzacji.000webhostapp.com/InwentaryzacjaAPI";

        public event EventHandler<ErrorEventArgs> ErrorEventHandler;


        #region Private

        private async Task<string> SendRequest(string address)
        {
            string result;
            int statusCode = 200;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri(BaseUrl + address);
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

        private async Task<bool> SendRequest(string address, StringContent content)
        {
            string result;
            int statusCode = 200;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri(BaseUrl + address);
                    var response = await App.clientHttp.PostAsync(uri, content);
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
        
        private async Task<bool> AuthorizationRequest(string address, StringContent content)
        {
            string result;
            int statusCode = 200;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri(BaseUrl + address);
                    var response = await App.clientHttp.PostAsync(uri, content);
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

        private T ConvertJSONToObject<T>(string json)
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

        private string ConvertDataToJSON<T>(T data)
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

        private StringContent PreperDataToSend(string data)
        {
            return new StringContent(data, Encoding.UTF8, "application/json");
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
                        Message = statement
                    };
                }
                
                e.SetErrorStatus(statusCode);
                ErrorEventHandler(this, e);
            }
        }

        #endregion Private


        #region Assets

        public async Task<AssetInfoEntity> getAssetInfo(int id)
        {
            var uri = $"/asset/getAssetInfo.php?id={id}";
            var response = await SendRequest(uri);

            return ConvertJSONToObject<AssetInfoEntity>(response);
        }

        public async Task<bool> CreateAsset(AssetPrototype asset)
        {
            string data = ConvertDataToJSON(asset);

            if (string.IsNullOrEmpty(data))
            {
                return false;
            }

            var uri = "/asset/addNewAsset.php";
            var content = PreperDataToSend(data);
            var response = await SendRequest(uri, content);

            return response;
        }


        #endregion Assets


        #region Localization

        public async Task<BuildingEntity[]> getBuildings()
        {
            var uri = "/building/getBuildings.php";
            var response = await SendRequest(uri);

            return ConvertJSONToObject<BuildingEntity[]>(response);
        }

        public async Task<RoomEntity[]> getRooms(int buildingId)
        {
            var uri = $"/room/getRooms.php?id={buildingId}";
            var response = await SendRequest(uri);

            return ConvertJSONToObject<RoomEntity[]>(response);
        }

        public async Task<bool> createBuilding(BuildingPrototype building)
        {
            var uri = "/report/addNewBuilding.php";
            string data = ConvertDataToJSON(building);

            if (data == null) return false;

            var content = PreperDataToSend(data);

            return await SendRequest(uri, content);
        }

        public async Task<bool> createRoom(RoomPropotype room)
        {
            var uri = "/report/addNewRoom.php";
            string data = ConvertDataToJSON(room);

            if (data == null) return false;

            var content = PreperDataToSend(data);

            return await SendRequest(uri, content);
        }
        

        #endregion Localization


        #region Report

        public async Task<ReportHeaderEntity[]> getReportHeaders()
        {
            var uri = "/report/getReportsHeaders.php";
            var response = await SendRequest(uri);

            return ConvertJSONToObject<ReportHeaderEntity[]>(response);
        }

        public async Task<ReportHeaderEntity> getReportHeader(int reportId)
        {
            var uri = $"/report/getReportHeader.php?id={reportId}";
            var response = await SendRequest(uri);

            return ConvertJSONToObject<ReportHeaderEntity>(response);
        }

        public async Task<ReportPosition[]> getReportPositions(int reportId)
        {
            var uri = $"/report/getReportPositions.php?id={reportId}";
            var response = await SendRequest(uri);

            return ConvertJSONToObject<ReportPosition[]>(response);
        }


        public async Task<bool> createReport(ReportPrototype report)
        {
            var uri = "/report/addNewReport.php";
            string data = ConvertDataToJSON(report);

            if (data == null) return false;

            var content = PreperDataToSend(data);

            return await SendRequest(uri, content);
        }

        #endregion Report


        #region User

        class LoginUserPrototype
        {
            public string login;
            public string password;

            public LoginUserPrototype(string login, string password)
            {
                this.login = login;
                this.password = password;
            }
        }

        public async Task<bool> LoginUser(string login, string password)
        {
            var uri = "/login/login.php";
            var data = ConvertDataToJSON(new LoginUserPrototype(login, password));
            var content = PreperDataToSend(data);

            return await AuthorizationRequest(uri, content);
        }

        #endregion User
    }
}
