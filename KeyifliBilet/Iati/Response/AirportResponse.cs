using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class AirportResponse
    {
        public IList<Airport> result { get; set; }
    }

    public class Airport
    {
        public string code { get; set; }
        public string name { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public string countryName { get; set; }
        public string countryCode { get; set; }
        public int timezone { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public bool city { get; set; }
    }
}