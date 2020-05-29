using Inwentaryzacja.Models;
using Newtonsoft.Json;
using System;
using System.Dynamic;
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
        static private readonly HttpClient ClientHttp = new HttpClient();

        static private readonly string BaseUrl = "https://aplikacja-do-inwentaryzacji.000webhostapp.com/api";

        public event EventHandler<ErrorEventArgs> ErrorEventHandler;

        private static bool TMPTOKEN = true;

        public APIController()
        {
            // TMP, ustawiam fake token na startcie

            if(TMPTOKEN)
            {
                ClientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "fake-token");
            }

            TMPTOKEN = false;
        }

        #region Private

        private async Task<string> SendRequestWithResponse(string address)
        {
            string result;
            int statusCode = 200;

                try
                {
                    var uri = new Uri(BaseUrl + address);
                    var response = await ClientHttp.GetAsync(uri);
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

            ErrorInvoke(result, statusCode);
            return null;
        }

        private async Task<bool> SendRequest(string address, StringContent content)
        {
            string result;
            int statusCode = 200;

                try
                {
                    var uri = new Uri(BaseUrl + address);
                    var response = await ClientHttp.PostAsync(uri, content);
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

            ErrorInvoke(result, statusCode);
            return false;
        }
        
        private async Task<bool> AuthorizationRequest(string address, StringContent content)
        {
            string result;
            int statusCode = 200;

                try
                {
                    var uri = new Uri(BaseUrl + address);
                    var response = await ClientHttp.PostAsync(uri, content);
                    result = await response.Content.ReadAsStringAsync();
                    statusCode = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        var header = response.Headers.GetValues("Authorization").First().ToString();
                        header = header.Remove(0, 7);
                        ClientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", header);

                        return true;
                    }
                }
                catch (Exception failConnection)
                {
                    result = "{\"message\":\"" + failConnection.Message + "\"}";
                    statusCode = 502;
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

        public async Task<AssetInfoEntity> getAssetInfo(int assetId)
        {
            var uri = $"/getAssetInfo/{assetId}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<AssetInfoEntity>(response);
        }

        public async Task<bool> CreateAsset(AssetPrototype asset)
        {
            string data = ConvertDataToJSON(asset);

            if (string.IsNullOrEmpty(data))
            {
                return false;
            }

            var uri = "/addNewAsset";
            var content = PreperDataToSend(data);
            var response = await SendRequest(uri, content);

            return response;
        }


        #endregion Assets


        #region Localization

        public async Task<BuildingEntity[]> getBuildings()
        {
            var uri = "/getBuildings";
            var response = await SendRequestWithResponse(uri);
           
            return ConvertJSONToObject<BuildingEntity[]>(response);
        }

        public async Task<RoomEntity[]> getRooms(int buildingId)
        {
            var uri = $"/getRooms/{buildingId}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<RoomEntity[]>(response);
        }

        public async Task<bool> createBuilding(BuildingPrototype building)
        {
            var uri = "/addNewBuilding";
            string data = ConvertDataToJSON(building);
            var content = PreperDataToSend(data);

            return await SendRequest(uri, content);
        }

        public async Task<bool> createRoom(RoomPropotype room)
        {
            var uri = "/addNewRoom";
            string data = ConvertDataToJSON(room);
            var content = PreperDataToSend(data);

            return await SendRequest(uri, content);
        }
        

        #endregion Localization


        #region Report

        public async Task<ReportHeaderEntity[]> getReportHeaders()
        {
            var uri = "/getReportsHeaders";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<ReportHeaderEntity[]>(response);
        }

        public async Task<ReportHeaderEntity> getReportHeader(int reportId)
        {
            var uri = $"/getReportHeader/{reportId}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<ReportHeaderEntity>(response);
        }

        public async Task<ReportPositionEntity[]> getReportPositions(int reportId)
        {
            var uri = $"/getReportPositions/{reportId}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<ReportPositionEntity[]>(response);
        }


        public async Task<bool> createReport(ReportPrototype report)
        {
            var uri = "/addNewReport";
            string data = ConvertDataToJSON(report);

            if (data == null) return false;

            var content = PreperDataToSend(data);

            return await SendRequest(uri, content);
        }

        #endregion Report


        #region Scanning

        public async Task<AssetEntity[]> getAssetsInRoom(int room_id)
        {
            var uri = $"/getAssetsInRoom/{room_id}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<AssetEntity[]>(response);
        }

        #endregion Scanning


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
            var uri = "/loginUser";
            var data = ConvertDataToJSON(new LoginUserPrototype(login, password));
            var content = PreperDataToSend(data);

            return await AuthorizationRequest(uri, content);
        }

        public string GetToken()
        {
            if (ClientHttp != null && ClientHttp.DefaultRequestHeaders != null && ClientHttp.DefaultRequestHeaders.Authorization!=null)
            {
                return ClientHttp.DefaultRequestHeaders.Authorization.Parameter;
            }

            return null;
        }

        public void SetToken(string token)
        {
            ClientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void DeleteToken()
        {
            if(ClientHttp!=null && ClientHttp.DefaultRequestHeaders !=null)
            {
                ClientHttp.DefaultRequestHeaders.Authorization = null;
            }
        }

        #endregion User
    }
}
