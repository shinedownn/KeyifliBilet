using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{          


    public class GetItineraryDetailResponseLeg
    {
        public string flightNo { get; set; }
        public string aircraft { get; set; }
        public string carrierCode { get; set; }
        public string carrierName { get; set; }
        public string operatorCode { get; set; }
        public string operatorName { get; set; }
        public string validatingCarrierCode { get; set; }
        public string departureAirport { get; set; }
        public string localDepartureDate { get; set; }
        public DateTime departureTime { get; set; }
        public string departureAirportName { get; set; }
        public string departureCityName { get; set; }
        public string arrivalAirport { get; set; }
        public string localArrivalDate { get; set; }
        public DateTime arrivalTime { get; set; }
        public string arrivalAirportName { get; set; }
        public string arrivalCityName { get; set; }
        public string numStops { get; set; }
        public int legDurationMinute { get; set; }
        public int waitTimeBeforeNextLeg { get; set; }
        public string operatingAirlineCode { get; set; }
    }

    public class GetItineraryDetailResponsePersonFare
    {
        public double total { get; set; }
        public double baseFare { get; set; }
        public double tax { get; set; }
        public double serviceFee { get; set; }
        public double supplement { get; set; }
        public double agencyCommission { get; set; }
        public string passangerType { get; set; }
    }

    public class GetItineraryDetailResponseDetail
    {
        public double price { get; set; }
        public double serviceFee { get; set; }
        public double tax { get; set; }
        public double suplement { get; set; }
    }

    public class GetItineraryDetailResponseFare
    {
        public double totalSingleAdultFare { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        [JsonProperty(PropertyName = "personFares")]
        public IList<GetItineraryDetailResponsePersonFare> personFares { get; set; }
        public IList<string> segmentNames { get; set; }
        public int freeSeatCount { get; set; }
        public GetItineraryDetailResponseDetail detail { get; set; }
    }

    public class GetItineraryDetailResponseResult
    {
        public string id { get; set; }
        public string providerKey { get; set; }
        public string pricingType { get; set; }
        public int packageId { get; set; }
        [JsonProperty(PropertyName = "legs")]
        public IList<GetItineraryDetailResponseLeg> legs { get; set; }
        [JsonProperty(PropertyName = "fares")]
        public IList<GetItineraryDetailResponseFare> fares { get; set; }
        public int segmentCount { get; set; }
        public int baggage { get; set; }
        public int flightTimeHour { get; set; }
        public int flightTimeMinute { get; set; }
        public double layoverTime { get; set; }
        public bool hasCip { get; set; }
        public bool dayCross { get; set; }
        public bool returnFlight { get; set; }
    }

    public class GetItineraryDetailResponse
    {
        [JsonProperty(PropertyName = "result")]
        public GetItineraryDetailResponseResult result { get; set; }
    }
}