using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class MakeTicketRequest
    {
        public string priceDetailId { get; set; }
        public string cip { get; set; }
        [JsonProperty(PropertyName ="contactInfo")]
        public TicketContactInfo contactInfo { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<TicketPassenger> passengers { get; set; }
        public string notes { get; set; }
        public double agencyExtra { get; set; }
    }

    public class TicketPassenger
    {
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender gender { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PessengerType type { get; set; }                
    }
    public class TicketContactInfo
    {
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
    }

    
}