using KeyifliBilet.Iati_Models.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace KeyifliBilet.Models
{
    public class Iati
    {
        private static string AuthCode = "74787740818A51EDFA7CA239910E4271";

        private static string URL = "http://testapi.iati.com/rest/test/sample/{0}";
        private static string Airports = "http://testapi.iati.com/rest/airports/{0}/{1}";
        private static string FlightSearch = "http://testapi.iati.com/rest/flightSearch/{0}";
        private static string Week = "http://testapi.iati.com/rest/getCalendar/{0}";



        public Iati()
        {
            URL = string.Format(URL, AuthCode);
        }
        public string GetAirports(string countryCode)
        {
            Airports = string.Format(Airports, AuthCode, countryCode);
            return CreateObject(Airports);
        }
        

        public string CreateObject(string url)
        {
            string response = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = null;
            request.Method = "POST";
            request.ContentType = "application/json";
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            response = responseReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return response;
        }

        public static string WeekSearch(RootObject data)
        {
          
            Week = string.Format(Week, AuthCode);
            string result = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Week);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new

            StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var f = JsonConvert.SerializeObject(data);
                streamWriter.Write(f);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
          
            return result;
        }

        public static string Search(Search search)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            FlightSearch = string.Format(FlightSearch, AuthCode);
            string result = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(FlightSearch);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            
            using (var streamWriter = new

            StreamWriter(httpWebRequest.GetRequestStream()))
            {
                


                var f = JsonConvert.SerializeObject(search);


                streamWriter.Write(f);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            watch.Stop();
            var elapsed = watch.Elapsed;
            return result;
        }
    }
}