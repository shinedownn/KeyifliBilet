using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class MultiLegPriceDetailRequest
    {
        public string searchId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency currency { get; set; }
        [JsonProperty(PropertyName = "fareReferences")]
        public IList<MultiLegFareReference> fareReferences { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<MultiLegPassenger> passengers { get; set; }
        public bool cip { get; set; }
        
    }
    public class MultiLegFareReference
    {
        public string itineraryId { get; set; }
        public FareType fareType { get; set; }
    }
    public class MultiLegPassenger
    {
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.PessengerType type { get; set; }
        public int count { get; set; }
    }
}