using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Inwentaryzacja.models
{
    class Building
    {
        public static string message = "";
        public int id { get; set; }
        public string name { get; set; }

        public static async Task<Building> findOneByID(int id)
        {
            Building building;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/building/read_one.php?id=" + id.ToString());
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        building = JsonConvert.DeserializeObject<Building>(content);
                        Building.message = "Succes";
                    }
                    else
                    {
                        //wrong request, asset does not exist
                        Building.message = await response.Content.ReadAsStringAsync();
                        return null;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    Building.message = failConnection.Message;
                    return null;
                }
            }
            else
            {
                //no internet connection
                Building.message = "no internet connection";
                return null;
            }

            return building;
        }

        public static async Task<List<Building>> findAll()
        {
            List<Building> buildingList;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/building/read.php");
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        buildingList = JsonConvert.DeserializeObject<List<Building>>(content);
                        Building.message = "Succes";
                    }
                    else
                    {
                        //wrong request
                        Building.message = await response.Content.ReadAsStringAsync();
                        return null;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    Building.message = failConnection.Message;
                    return null;
                }
            }
            else
            {
                //no internet connection
                Building.message = "no internet connection";
                return null;
            }

            return buildingList;
        }

        public static async Task<bool> sendBuilding(Building building)
        {
            if (!String.IsNullOrEmpty(building.name))
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/building/create.php");
                        var data = JsonConvert.SerializeObject(building);
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var response = await App.clientHttp.PostAsync(uri, content);

                        Building.message = await response.Content.ReadAsStringAsync();

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
                        Building.message = failConnection.Message;
                        return false;
                    }
                }
                else
                {
                    //no internet connection
                    Building.message = "no internet connection";
                    return false;
                }
            }

            //wrong data
            Building.message = "wrong data";
            return false;
        }

        public static async Task<bool> deleteBuilding(int id)
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/building/delete.php");
                    var data = "{\"id\":\"" + id + "\"}";
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await App.clientHttp.PostAsync(uri, content);

                    Building.message = await response.Content.ReadAsStringAsync();

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
                    Building.message = failConnection.Message;
                    return false;
                }
            }
            else
            {
                //no internet connection
                Building.message = "no internet connection";
                return false;
            }

            return false;
        }
    }
}
