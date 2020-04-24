using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Inwentaryzacja.models
{
    class Report
    {
        public static string message = "";
        public int id { get; set; }
        public string name { get; set; }
        public int room { get; set; }
        public DateTime create_data { get; set; }
        public int owner { get; set; }

        public static async Task<Report> findOneByID(int id)
        {
            Report report;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/report/read_one.php?id=" + id.ToString());
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        report = JsonConvert.DeserializeObject<Report>(content);
                        Report.message = "Succes";
                    }
                    else
                    {
                        //wrong request, asset does not exist
                        Report.message = await response.Content.ReadAsStringAsync();
                        return null;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    Report.message = failConnection.Message;
                    return null;
                }
            }
            else
            {
                //no internet connection
                Report.message = "no internet connection";
                return null;
            }

            return report;
        }

        public static async Task<List<Report>> findAll()
        {
            List<Report> reportList;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/report/read.php");
                    var response = await App.clientHttp.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        reportList = JsonConvert.DeserializeObject<List<Report>>(content);
                        Report.message = "Succes";
                    }
                    else
                    {
                        //wrong request
                        Report.message = await response.Content.ReadAsStringAsync();
                        return null;
                    }

                }
                catch (Exception failConnection)
                {
                    //server does not exist, cannot conver data
                    Report.message = failConnection.Message;
                    return null;
                }
            }
            else
            {
                //no internet connection
                Report.message = "no internet connection";
                return null;
            }

            return reportList;
        }

        public static async Task<bool> sendReport(Report report)
        {
            if (!String.IsNullOrEmpty(report.name) && report.room!=0 && report.create_data!=null && report.owner!=0)
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/report/create.php");
                        var data = JsonConvert.SerializeObject(report);
                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var response = await App.clientHttp.PostAsync(uri, content);

                        Report.message = await response.Content.ReadAsStringAsync();

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
                        Report.message = failConnection.Message;
                        return false;
                    }
                }
                else
                {
                    //no internet connection
                    Report.message = "no internet connection";
                    return false;
                }
            }

            //wrong data
            Report.message = "wrong data";
            return false;
        }

        public static async Task<bool> deleteReport(int id)
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var uri = new Uri("http://maciejdominiak.000webhostapp.com/InwentaryzacjaAPI/report/delete.php");
                    var data = "{\"id\":\"" + id + "\"}";
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await App.clientHttp.PostAsync(uri, content);

                    Report.message = await response.Content.ReadAsStringAsync();

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
                    Report.message = failConnection.Message;
                    return false;
                }
            }
            else
            {
                //no internet connection
                Report.message = "no internet connection";
                return false;
            }

            return false;
        }
    }
}
