using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Inwentaryzacja.models
{
    class User
    {
        static public string message = "";

        public static async Task<bool> createUser(string login, string password)
        {
            if (!String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(password))
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/creator/user_creator.php");
                        var data = "{\"login\":\"" + login + "\", \"password\":\"" + password + "\"}";
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var response = await App.clientHttp.PostAsync(uri, content);

                        User.message = await response.Content.ReadAsStringAsync();

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
                        User.message = failConnection.Message;
                        return false;
                    }
                }
                else
                {
                    //no internet connection
                    User.message = "no internet connection";
                    return false;
                }
            }

            //wrong data
            User.message = "wrong data";
            return false;
        }

    }
}
