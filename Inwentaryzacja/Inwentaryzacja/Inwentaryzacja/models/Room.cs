using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Inwentaryzacja.models
{
    class Room
    {
        public static string message = "";
        public int id { get; set; }
        public string name { get; set; }
        public int building { get; set; }


        public static async Task<Room> findOneByID(int id)
        {
            Room room;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/room/read_one.php?id=" + id.ToString());
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        room = JsonConvert.DeserializeObject<Room>(content);
                        Room.message = "Succes";
                    }
                    else
                    {
                        //wrong request, asset does not exist
                        Room.message = await response.Content.ReadAsStringAsync();
                        return null;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    Room.message = failConnection.Message;
                    return null;
                }
            }
            else
            {
                //no internet connection
                Room.message = "no internet connection";
                return null;
            }

            return room;
        }

        public static async Task<List<Room>> findAll()
        {
            List<Room> roomList;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/room/read.php");
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        roomList = JsonConvert.DeserializeObject<List<Room>>(content);
                        Room.message = "Succes";
                    }
                    else
                    {
                        //wrong request
                        Room.message = await response.Content.ReadAsStringAsync();
                        return null;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    Room.message = failConnection.Message;
                    return null;
                }
            }
            else
            {
                //no internet connection
                Room.message = "no internet connection";
                return null;
            }

            return roomList;
        }

        public static async Task<bool> sendRoom(Room room)
        {
            if (!String.IsNullOrEmpty(room.name) && room.building!=0)
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/room/create.php");
                        var data = JsonConvert.SerializeObject(room);
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var response = await App.clientHttp.PostAsync(uri, content);

                        Room.message = await response.Content.ReadAsStringAsync();

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
                        Room.message = failConnection.Message;
                        return false;
                    }
                }
                else
                {
                    //no internet connection
                    Room.message = "no internet connection";
                    return false;
                }
            }

            //wrong data
            Room.message = "wrong data";
            return false;
        }

        public static async Task<bool> deleteRoom(int id)
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/room/delete.php");
                    var data = "{\"id\":\"" + id + "\"}";
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await App.clientHttp.PostAsync(uri, content);

                    Room.message = await response.Content.ReadAsStringAsync();

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
                    Room.message = failConnection.Message;
                    return false;
                }
            }
            else
            {
                //no internet connection
                Room.message = "no internet connection";
                return false;
            }

            return false;
        }
    }
}
