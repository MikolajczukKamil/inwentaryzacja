using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Inwentaryzacja.models
{
    public class AssetType
    {
        public int id { get; set; }
        public string letter { get; set; }
        public string name { get; set; }

        public static async Task<AssetType> findOneByID(int id)
        {
            AssetType assetType;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/asset_type/read_one.php?id=" + id.ToString());
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        assetType = JsonConvert.DeserializeObject<AssetType>(content);
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

            return assetType;
        }

        public static async Task<List<AssetType>> findAll()
        {
            List<AssetType> assetTypeList;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/asset_type/read.php");
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        assetTypeList = JsonConvert.DeserializeObject<List<AssetType>>(content);
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

            return assetTypeList;
        }

        public static async Task<bool> sendAsset(AssetType assetType)
        {
            if (!String.IsNullOrEmpty(assetType.name) && !String.IsNullOrEmpty(assetType.letter))
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/asset_type/create.php");
                        var data = JsonConvert.SerializeObject(assetType);
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var response = await App.clientHttp.PostAsync(uri, content);

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            //wrong request
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
            }

            //wrong data
            return false;
        }

        public static async Task<bool> deleteAsset(int id)
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/asset_type/delete.php");
                    var data = "{\"id\":\"" + id + "\"}";
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await App.clientHttp.PostAsync(uri, content);

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
