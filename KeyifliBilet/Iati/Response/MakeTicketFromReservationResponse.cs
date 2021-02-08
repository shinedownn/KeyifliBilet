using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class MakeTicketFromReservationResponsePassenger
    {
        public int eticket { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
        public bool isTurkishCitizen { get; set; }
    }

    public class MakeTicketFromReservationResponseResult
    {
        public string pnr { get; set; }
        public int orderId { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<MakeTicketFromReservationResponsePassenger> passengers { get; set; }
        public bool multiProvider { get; set; }
    }

    public class MakeTicketFromReservationResponse
    {
        [JsonProperty(PropertyName ="result")]
        public MakeTicketFromReservationResponseResult result { get; set; }
    }
}