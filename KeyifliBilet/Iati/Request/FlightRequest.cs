using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{


    public class FlightRequest
    {
        public string fromAirport { get; set; }
        public string toAirport { get; set; }
        public bool allinFromCity { get; set; }
        public bool allinToCity { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.Currency currency { get; set; }
        //public bool allinToCity { get; set; }
        public string fromDate { get; set; }
        public string returnDate { get; set; }
        public string adult { get; set; }
        public string child { get; set; }
        public string infant { get; set; }
        //public string senior { get; set; }
        //public string student { get; set; }
        //public string young { get; set; }
        //public string military { get; set; }
        //public string disable { get; set; }
        public bool cip { get; set; }
        //[JsonConverter(typeof(StringEnumConverter))]
        //public FilterProvider filterProvider { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ClassType? classType { get; set; }
        public bool usePersonFares { get; set; }
        public bool getBaggageInfo { get; set; }        
    }
}