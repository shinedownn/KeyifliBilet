using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class GetItineraryDetailRequest
    {
        public string id { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency? currency { get; set; }
    }
}