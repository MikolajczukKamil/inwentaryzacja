using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Inwentaryzacja.models
{
    public class Asset
    {
        public int id { get; set; }
        public string name { get; set; }
        public int assetType { get; set; }

        public static async Task<Asset> findOneByID(int id)
        {
            Asset asset;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/asset/read_one.php?id=" + id.ToString());
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        asset = JsonConvert.DeserializeObject<Asset>(content);
                    }
                    else
                    {
                        //wrong request, asset does not exist
                        return null;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    return null;
                }
            }
            else
            {
                //no internet connection
                return null;
            }

            return asset;
        }

        public static async Task<List<Asset>> findAll()
        {
            List<Asset> assetList;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/asset/read.php");
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        assetList = JsonConvert.DeserializeObject<List<Asset>>(content);
                    }
                    else
                    {
                        //wrong request
                        return null;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    return null;
                }
            }
            else
            {
                //no internet connection
                return null;
            }

            return assetList;
        }

        public static async Task<bool> sendAsset(Asset asset)
        {
            if (!String.IsNullOrEmpty(asset.name) && asset.assetType != 0)
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/asset/create.php");
                        var data = JsonConvert.SerializeObject(asset);
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var response = await App.clientHttp.PostAsync(uri, content);

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            //wrong request
                            Debug.WriteLine("moje: złe zapytanie, "+response.Content.ReadAsStringAsync().Result);
                            return false;
                        }

                    }
                    catch (Exception failConnection)
                    {
                        //server does not exist, cannot conver data
                        Debug.WriteLine("moje: serwer nie odpowiada, "+failConnection.Message);
                        return false;
                    }
                }
                else
                {
                    //no internet connection
                    Debug.WriteLine("moje: brak internetu");
                    return false;
                }
            }

            //wrong data
            Debug.WriteLine("moje: złe dane");
            return false;
        }

        public static async Task<bool> deleteAsset(int id)
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/asset/delete.php");
                    var data = "{\"id\":\"" + id + "\"}";
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await App.clientHttp.PostAsync(uri,content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        //wrong request, asset does not exist
                        return false;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    return false;
                }
            }
            else
            {
                //no internet connection
                return false;
            }

            return false;
        }
    }
}
