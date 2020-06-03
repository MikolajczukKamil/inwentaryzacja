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
    /// <summary>
    /// Klasa do obsługi interakcji z API
    /// </summary>
    public class APIController
    {
        /// <summary>
        /// Obiekt, przez który wysyłane są zapytania do API i odbierane odpowiedzi
        /// </summary>
        static private readonly HttpClient ClientHttp = new HttpClient();

        /// <summary>
        /// URL serwera API
        /// </summary>
        static private readonly string BaseUrl = "https://aplikacja-do-inwentaryzacji.000webhostapp.com/api";

        /// <summary>
        /// Event handler dla błędow występujących podczas zapytań do API
        /// </summary>
        public event EventHandler<ErrorEventArgs> ErrorEventHandler;


        #region Private
        
        /// <summary>
        /// Asynchronicznie wysyła podane zapytanie do API i zwraca odpowiedź
        /// </summary>
        /// <param name="address">Adres zapytania</param>
        /// <returns>Odpowiedź na zapytanie w formacie JSON</returns>
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

        /// <summary>
        /// Asynchronicznie wysyła podane zapytanie do API wraz z dodatkową zawartością
        /// </summary>
        /// <param name="address">Adres zapytania</param>
        /// <param name="content">Zawartość zapytania</param>
        /// <returns>Czy udało się wykonać zapytanie</returns>
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
        
        /// <summary>
        /// Asynchronicznie wysyła zapytanie o logowanie
        /// </summary>
        /// <param name="address">Adres zapytania logowania</param>
        /// <param name="content">Dane logowania w formacie JSON</param>
        /// <returns>Czy udało się zalogować</returns>
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

        /// <summary>
        /// Deserializuje JSON z powrotem w obiekt
        /// </summary>
        /// <param name="json">Zserializowany obiekt JSON</param>
        /// <typeparam name="T">Typ obiektu</typeparam>
        /// <returns>Zdeserializowany obiekt</returns>
        private T ConvertJSONToObject<T>(string json)
        {
            T entity = default;

            if(json!=null)
            {
                try
                {
                    entity = JsonConvert.DeserializeObject<T>(json);
                }
                catch (Exception)
                {
                    entity = default;
                    ErrorInvoke("{\"message\":\"Cannot convert data.\"}", 410);
                }
            }

            return entity;
        }

        /// <summary>
        /// Serializuje obiekt w JSON
        /// </summary>
        /// <param name="data">Obiekt do serializacji</param>
        /// <typeparam name="T">Typ obiektu</typeparam>
        /// <returns>Zserializowany obiekt JSON</returns>
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
                ErrorInvoke("{\"message\":\"Cannot convert data.\"}", 411);
            }

            return entity;
        }

        /// <summary>
        /// Przygotowywuje dane do wysłania 
        /// </summary>
        /// <param name="data">Dane</param>
        /// <returns>Przygotowane dane</returns>
        private StringContent PreperDataToSend(string data)
        {
            return new StringContent(data, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Wywołuje wystąpienie błędu
        /// </summary>
        /// <param name="statement">Komunikat</param>
        /// <param name="statusCode">Kod statusu</param>
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

        /// <summary>
        /// Asynchronicznie zwraca z bazy informacje o wybranym środku trwałym
        /// Jeżeli wystąpi błąd, zostanie wywołany event z błędem i zostanie zwrócony null
        /// </summary>
        /// <param name="assetId">ID środka trwałego</param>
        /// <returns>Informacje o środku trwałym</returns>
        public async Task<AssetInfoEntity> getAssetInfo(int assetId)
        {
            var uri = $"/getAssetInfo/{assetId}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<AssetInfoEntity>(response);
        }

        /// <summary>
        /// Asynchronicznie tworzy i dodaje do bazy nowy środek trwały z podanego opisu
        /// </summary>
        /// <param name="asset">Opis środka trwałego</param>
        /// <returns>Czy udało się stworzyć i dodać do bazy nowy środek trwały</returns>
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

        /// <summary>
        /// Asynchronicznie zwraca tabelę budynków w bazie
        /// Jeżeli wystąpi błąd, zostanie wywołany event z błędem i zostanie zwrócony null
        /// </summary>
        /// <returns>Tabela budynków</returns>
        public async Task<BuildingEntity[]> getBuildings()
        {
            var uri = "/getBuildings";
            var response = await SendRequestWithResponse(uri);
           
            return ConvertJSONToObject<BuildingEntity[]>(response);
        }

        /// <summary>
        /// Asynchronicznie zwraca tablicę pokoi w wybranym budynku
        /// Jeżeli wystąpi błąd, zostanie wywołany event z błędem i zostanie zwrócony null
        /// </summary>
        /// <param name="buildingId">ID budynku</param>
        /// <returns>Tablica pokoi</returns>
        public async Task<RoomEntity[]> getRooms(int buildingId)
        {
            var uri = $"/getRooms/{buildingId}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<RoomEntity[]>(response);
        }

        /// <summary>
        /// Asynchronicznie tworzy i dodaje do bazy nowy budynek z podanego opisu
        /// </summary>
        /// <param name="building">Opis budynku</param>
        /// <returns>Czy udało się stworzyć i dodać do bazy nowy budynek</returns>
        public async Task<bool> createBuilding(BuildingPrototype building)
        {
            var uri = "/addNewBuilding";
            string data = ConvertDataToJSON(building);
            var content = PreperDataToSend(data);

            return await SendRequest(uri, content);
        }

        /// <summary>
        /// Asynchronicznie tworzy i dodaje do bazy nowy pokój z podanego opisu
        /// </summary>
        /// <param name="room">Opis pokoju</param>
        /// <returns>Czy udało się stworzyć i dodać do bazy nowy pokój</returns>
        public async Task<bool> createRoom(RoomPropotype room)
        {
            var uri = "/addNewRoom";
            string data = ConvertDataToJSON(room);
            var content = PreperDataToSend(data);

            return await SendRequest(uri, content);
        }
        

        #endregion Localization


        #region Report

        /// <summary>
        /// Asynchronicznie zwraca tablicę wszystkich nagłówków raportów
        /// Jeżeli wystąpi błąd, zostanie wywołany event z błędem i zostanie zwrócony null
        /// </summary>
        /// <returns>Tablica wszystkich nagłówków raportów</returns>
        public async Task<ReportHeaderEntity[]> getReportHeaders()
        {
            var uri = "/getReportsHeaders";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<ReportHeaderEntity[]>(response);
        }

        /// <summary>
        /// Asynchronicznie zwraca wybrany nagłówek raportu
        /// Jeżeli wystąpi błąd, zostanie wywołany event z błędem i zostanie zwrócony null
        /// </summary>
        /// <param name="reportId">ID raportu</param>
        /// <returns>Wybrany nagłówek raportu</returns>
        public async Task<ReportHeaderEntity> getReportHeader(int reportId)
        {
            var uri = $"/getReportHeader/{reportId}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<ReportHeaderEntity>(response);
        }

        /// <summary>
        /// Asynchronicznie zwraca tablicę środków trwałych wybranego raportu
        /// Jeżeli wystąpi błąd, zostanie wywołany event z błędem i zostanie zwrócony null
        /// </summary>
        /// <param name="reportId">ID raportu</param>
        /// <returns>Tablica środków trwałych wybranego raportu</returns>
        public async Task<ReportPositionEntity[]> getReportPositions(int reportId)
        {
            var uri = $"/getReportPositions/{reportId}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<ReportPositionEntity[]>(response);
        }
        
        /// <summary>
        /// Asynchronicznie tworzy i dodaje do bazy nowy raport z podanego opisu
        /// </summary>
        /// <param name="report">Opis raportu</param>
        /// <returns>Czy udało się stworzyć i dodać do bazy nowy raport</returns>
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

        /// <summary>
        /// Asynchronicznie zwraca tablicę środków trwałych w wybranym pokoju
        /// Jeżeli wystąpi błąd, zostanie wywołany event z błędem i zostanie zwrócony null
        /// </summary>
        /// <param name="room_id">ID pokoju</param>
        /// <returns>Tablicę środków trwałych w wybranym pokoju</returns>
        public async Task<AssetEntity[]> getAssetsInRoom(int room_id)
        {
            var uri = $"/getAssetsInRoom/{room_id}";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<AssetEntity[]>(response);
        }


        public async Task<ScanEntity[]> getScans()
        {
            var uri = $"/getScans";
            var response = await SendRequestWithResponse(uri);

            return ConvertJSONToObject<ScanEntity[]>(response);
        }

        #endregion Scanning


        #region User

        /// <summary>
        /// Klasa zawierająca dane logowania użytkownika
        /// </summary>
        class LoginUserPrototype
        {
            /// <summary>
            /// Login użytkownika
            /// </summary>
            public string login;
            
            /// <summary>
            /// Hasło użytykownika
            /// </summary>
            public string password;

            /// <summary>
            /// Konstruktor klasy
            /// </summary>
            /// <param name="login">Login użytkownika</param>
            /// <param name="password">Hasło użytykownika</param>
            public LoginUserPrototype(string login, string password)
            {
                this.login = login;
                this.password = password;
            }
        }

        /// <summary>
        /// Asynchronicznie autentyfikuje i loguje podanego użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło użytykownika</param>
        /// <returns>Czy udało się zalogować</returns>
        public async Task<bool> LoginUser(string login, string password)
        {
            var uri = "/loginUser";
            var data = ConvertDataToJSON(new LoginUserPrototype(login, password));
            var content = PreperDataToSend(data);

            return await AuthorizationRequest(uri, content);
        }

        /// <summary>
        /// Zwraca token
        /// Jeżeli wystąpi błąd, zostanie wywołany event z błędem i zostanie zwrócony null
        /// </summary>
        /// <returns>Ciąg znaków tokena</returns>
        public string GetToken()
        {
            if (ClientHttp != null && ClientHttp.DefaultRequestHeaders != null && ClientHttp.DefaultRequestHeaders.Authorization!=null)
            {
                return ClientHttp.DefaultRequestHeaders.Authorization.Parameter;
            }

            return null;
        }

        /// <summary>
        /// Ustawia token
        /// </summary>
        /// <param name="token">Ciąg znaków tokena</param>
        public void SetToken(string token)
        {
            ClientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Usuwa token
        /// </summary>
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
