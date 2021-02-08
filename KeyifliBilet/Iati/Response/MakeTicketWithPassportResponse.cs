using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class MakeTicketWithPassportResponsePassenger
    {
        public string eticket { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
    }

    public class MakeTicketWithPassportResponseResult
    {
        public string pnr { get; set; }
        public int orderId { get; set; }
        [JsonProperty(PropertyName = "pessengers")]
        public IList<MakeTicketWithPassportResponsePassenger> passengers { get; set; }
    }

    public class MakeTicketWithPassportResponse
    {
        [JsonProperty(PropertyName = "result")]
        public MakeTicketWithPassportResponseResult result { get; set; }
    }
}