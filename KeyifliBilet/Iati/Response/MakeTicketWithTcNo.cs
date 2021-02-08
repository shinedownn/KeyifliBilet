using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class MakeTicketWithTcNoResponsePassenger
    {
        public string eticket { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
    }

    public class MakeTicketWithTcNoResponseResult
    {
        public string pnr { get; set; }
        public int orderId { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<MakeTicketWithTcNoResponsePassenger> passengers { get; set; }
    }

    public class MakeTicketWithTcNoResponse
    {
        [JsonProperty(PropertyName = "result")]
        public MakeTicketWithTcNoResponseResult result { get; set; }
    }
}