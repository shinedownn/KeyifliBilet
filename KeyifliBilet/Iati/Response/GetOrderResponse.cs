using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class GetOrderResponseBookingInfo
    {
        public string pnr { get; set; }
    }

    public class GetOrderResponseLeg
    {
        public string operatorName { get; set; }
        public string departureAirport { get; set; }
        public DateTime departureTime { get; set; }
        public string localDepartureDate { get; set; }
        public string arrivalAirport { get; set; }
        public DateTime arrivalTime { get; set; }
        public string localArrivalDate { get; set; }
        public string flightNo { get; set; }
        public string aircraft { get; set; }
        public string providerKey { get; set; }
        public string classCode { get; set; }
        public string operatingProviderName { get; set; }
        public string operatingIATA { get; set; }
        public string departureAirportName { get; set; }
        public string departureCityCode { get; set; }
        public string departureCityName { get; set; }
        public string departureCountryCode { get; set; }
        public string arrivalAirportName { get; set; }
        public string arrivalCityCode { get; set; }
        public string arrivalCityName { get; set; }
        public string arrivalCountryCode { get; set; }
    }

    public class GetOrderResponsePassenger
    {
        public string eticket { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
        public string gender { get; set; }
        public string persontype { get; set; }
        public double serviceFee { get; set; }
        public double totalPrice { get; set; }
        public double agencyComission { get; set; }
        public double tax { get; set; }
        public string currency { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }

    public class GetOrderResponseResult
    {
        public int orderId { get; set; }
        public string provider { get; set; }
        public DateTime creationDate { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public string contactGsm { get; set; }
        public string contactEmail { get; set; }
        public int pax { get; set; }
        [JsonProperty(PropertyName = "bookingInfo")]
        public GetOrderResponseBookingInfo bookingInfo { get; set; }
        [JsonProperty(PropertyName = "legs")]
        public IList<GetOrderResponseLeg> legs { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<GetOrderResponsePassenger> passengers { get; set; }
    }

    public class GetOrderResponse
    {
        [JsonProperty(PropertyName ="result")]
        public GetOrderResponseResult result { get; set; }
    }

}