using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class MakeTicketWithFoidResponsePassenger
    {
        public string eticket { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
    }

    public class MakeTicketWithFoidResponseResult
    {
        public string pnr { get; set; }
        public int orderId { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<MakeTicketWithFoidResponsePassenger> passengers { get; set; }
    }

    public class MakeTicketWithFoidResponse
    {
        [JsonProperty(PropertyName = "result")]
        public MakeTicketWithFoidResponseResult result { get; set; }
    }
}