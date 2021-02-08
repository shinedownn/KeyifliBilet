using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
   


    public class MakeDummyTicketResponsePassenger
    {
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
        public string eticket { get; set; }
        public bool turkishCitizen { get; set; }
    }

    public class MakeDummyTicketResponseResult
    {
        public string pnr { get; set; }
        public int orderId { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public List<MakeDummyTicketResponsePassenger> passengers { get; set; }
    }

    public class MakeDummyTicketResponse
    {
        [JsonProperty(PropertyName = "result")]
        public MakeDummyTicketResponseResult result { get; set; }
    }
}