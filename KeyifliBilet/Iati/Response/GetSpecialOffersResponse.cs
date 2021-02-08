using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class GetSpecialOffersResponseReturnItinerary
    {
        public string providerKey { get; set; }
        public string firstLegFlightNo { get; set; }
        public string operatorName { get; set; }
        public string departureCityName { get; set; }
        public string departureAirportName { get; set; }
        public string departureAirportCode { get; set; }
        public DateTime departureTime { get; set; }
        public string arrivalCityName { get; set; }
        public string arrivalAirportName { get; set; }
        public string arrivalAirportCode { get; set; }
        public DateTime arrivalTime { get; set; }
        public int totalSingleAdultFare { get; set; }
        public string fareCurrency { get; set; }
        public string fareType { get; set; }
        public int tripDurationHour { get; set; }
        public int tripDurationMinute { get; set; }
        public int transferCount { get; set; }
        public bool returnFlight { get; set; }
    }

    public class GetSpecialOffersResponseCharterList
    {
        public string providerKey { get; set; }
        public string firstLegFlightNo { get; set; }
        public string operatorName { get; set; }
        public string departureCityName { get; set; }
        public string departureAirportName { get; set; }
        public string departureAirportCode { get; set; }
        public DateTime departureTime { get; set; }
        public string arrivalCityName { get; set; }
        public string arrivalAirportName { get; set; }
        public string arrivalAirportCode { get; set; }
        public DateTime arrivalTime { get; set; }
        public double totalSingleAdultFare { get; set; }
        public string fareCurrency { get; set; }
        public string fareType { get; set; }
        public int tripDurationHour { get; set; }
        public int tripDurationMinute { get; set; }
        public int transferCount { get; set; }
        public bool returnFlight { get; set; }
        [JsonProperty(PropertyName = "returnItinerary")]
        public GetSpecialOffersResponseReturnItinerary returnItinerary { get; set; }
    }

    public class GetSpecialOffersResponseResult
    {
        [JsonProperty(PropertyName = "charterList")]
        public IList<GetSpecialOffersResponseCharterList> charterList { get; set; }
    }

    public class GetSpecialOffersResponse
    {
        [JsonProperty(PropertyName = "result")]
        public GetSpecialOffersResponseResult result { get; set; }
    }
}