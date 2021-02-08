using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Models
{
    public class Airport
    {
        public string code { get; set; }
        public string name { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public string countryName { get; set; }
        public string countryCode { get; set; }
        public int timezone { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public bool city { get; set; }
    }
}