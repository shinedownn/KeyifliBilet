using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class MakeTicketWithTCNORequest
    {
        public string priceDetailId { get; set; }
        public string cip { get; set; }
        [JsonProperty(PropertyName ="contactInfo")]
        public TicketwithTCNOContactInfo contactInfo { get; set; }
        [JsonProperty(PropertyName ="passengers")]
        public IList<TicketWithTCNOPassenger> passengers { get; set; }
        public string notes { get; set; }
    }
    public class TicketwithTCNOContactInfo
    {
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
    }
    public class TicketWithTCNOPassenger
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string birthdate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender gender { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PessengerType type { get; set; }
        public string identityNo { get; set; }
        public string turkishCitizen { get; set; }
    }
}