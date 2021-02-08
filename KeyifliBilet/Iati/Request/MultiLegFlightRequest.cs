using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class MultiLegFlightRequest
    {
        public int adult { get; set; }
        public int child { get; set; }
        public int infant { get; set; }
        public bool cip { get; set; }
        public bool getBaggageInfo { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ClassType classType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency currency { get; set; }
        [JsonProperty(PropertyName = "hops")]
        public IList<MultiLegHop> hops { get; set; }
        public bool usePersonFares { get; set; }
        //[JsonConverter(typeof(StringEnumConverter))]
        //public FilterProvider filterProviders { get; set; }        
    }
    
    public class MultiLegHop
    {
        public string fromCode { get; set; }
        public string toCode { get; set; }
        public string date { get; set; }
    }
}