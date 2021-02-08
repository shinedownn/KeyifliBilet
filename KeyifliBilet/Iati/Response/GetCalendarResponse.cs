using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class GetCalendarResponseLeg
    {
        public string flightNumber { get; set; }
        public string airlineCode { get; set; }
    }

    public class GetCalendarResponseDeparture
    {
        public string fromAirport { get; set; }
        public string toAirport { get; set; }
        public DateTime date { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public int fareType { get; set; }
        public string providerKey { get; set; }
        [JsonProperty(PropertyName = "legs")]
        public IList<GetCalendarResponseLeg> legs { get; set; }
    }    

    public class GetCalendarResponseReturn
    {
        public string fromAirport { get; set; }
        public string toAirport { get; set; }
        public DateTime date { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public int fareType { get; set; }
        public string providerKey { get; set; }
        [JsonProperty(PropertyName = "legs")]
        public IList<GetCalendarResponseLeg> legs { get; set; }
    }

    public class GetCalendarResponseResult
    {
        public bool success { get; set; }
        [JsonProperty(PropertyName = "departures")]
        public IList<GetCalendarResponseDeparture> departures { get; set; }
        [JsonProperty(PropertyName = "returns")]
        public IList<GetCalendarResponseReturn> returns { get; set; }
    }

    public class GetCalendarResponse
    {
        [JsonProperty(PropertyName = "result")]
        public GetCalendarResponseResult result { get; set; }
    }
}