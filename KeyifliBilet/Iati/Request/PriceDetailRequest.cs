using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class PriceDetailRequest
    {
        public string searchId { get; set; }
        [JsonProperty(PropertyName = "fareRefereces")]
        public IList<PriceDetailFareReferences> fareRefereces { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<PriceDetailPassenger> passengers { get; set; }
        public bool cip { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency currency { get; set; }
       
    }

    public class PriceDetailFareReferences
    {
        public string itineraryId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public FareType fareType { get; set; }
    }
    public class PriceDetailPassenger
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.PessengerType type { get; set; }
        public int count { get; set; }
    }
}