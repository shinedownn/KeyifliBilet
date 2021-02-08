using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class MakeTicketWithFoidRequest
    {
        public string priceDetailId { get; set; }
        public string cip { get; set; }
        [JsonProperty(PropertyName ="contactInfo")]
        public TicketWithFoidContactInfo contactInfo { get; set;}
        [JsonProperty(PropertyName ="passengers")]
        public IList<TicketWithFoidPassenger> passengers { get; set; }
        public string notes { get; set; }
    }
    public class TicketWithFoidContactInfo
    {
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
    }
    public class TicketWithFoidPassenger
    {
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender gender { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PessengerType type { get; set; }
        [JsonProperty(PropertyName ="foid")]
        public TicketWithFoid foid { get; set; }
    }

    public class TicketWithFoid
    {
        public string no { get; set; }
        public string citizenhipCountry { get; set; }
    }
}