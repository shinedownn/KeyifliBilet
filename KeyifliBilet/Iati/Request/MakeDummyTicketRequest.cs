using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class MakeDummyTicketContactInfo
    {
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
    }

    public class MakeDummyTicketPassenger
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string birthdate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender gender { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PessengerType type { get; set; }
        public string turkishCitizen { get; set; }
        public string identityNo { get; set; }
    }

    public class MakeDummyTicketRequest
    {
        public string notes { get; set; }
        public string priceDetailId { get; set; }
        public string cip { get; set; }
        public int agencyExtra { get; set; }
        public MakeDummyTicketContactInfo contactInfo { get; set; }
        public IList<MakeDummyTicketPassenger> passengers { get; set; }
        public bool returnEticket { get; set; }
        public bool returnError { get; set; }
    }
}