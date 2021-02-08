using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class MakeTicketWithPassportContactInfo
    {
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
    }

    public class MakeTicketWithPassportPassport
    {
        public string no { get; set; }
        public string serial { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime endDate { get; set; }
        public string citizenhipCountry { get; set; }
        public string issueCountry { get; set; }
    }

    public class MakeTicketWithPassportPassenger
    {
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender gender { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PessengerType type { get; set; }
        public MakeTicketWithPassportPassport passport { get; set; }
    }

    public class MakeTicketWithPassportRequest
    {
        public string priceDetailId { get; set; }
        public string cip { get; set; }
        public MakeTicketWithPassportContactInfo contactInfo { get; set; }
        public IList<MakeTicketWithPassportPassenger> passengers { get; set; }
        public string notes { get; set; }
    }
}