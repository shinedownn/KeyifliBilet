using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class GetOrderListResponseCancellationInfo
    {
        public string pnr { get; set; }
    }

    public class GetOrderListResponseLeg
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

    public class GetOrderListResponsePassenger
    {
        public string eticket { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthdate { get; set; }
        public string gender { get; set; }
        public string persontype { get; set; }
        public int serviceFee { get; set; }
        public double totalPrice { get; set; }
        public int agencyComission { get; set; }
        public double tax { get; set; }
        public string currency { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string cancelStatus { get; set; }
    }

    public class GetOrderListResponseOrderModel
    {
        public int orderId { get; set; }
        public string provider { get; set; }
        public DateTime creationDate { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public string contactGsm { get; set; }
        public string contactEmail { get; set; }
        public int pax { get; set; }
        [JsonProperty(PropertyName = "cancellationInfo")]
        public GetOrderListResponseCancellationInfo cancellationInfo { get; set; }
        [JsonProperty(PropertyName = "legs")]
        public IList<GetOrderListResponseLeg> legs { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<GetOrderListResponsePassenger> passengers { get; set; }
    }

    public class GetOrderListResponseResult
    {
        [JsonProperty(PropertyName = "orderModels")]
        public IList<GetOrderListResponseOrderModel> orderModels { get; set; }
    }

    public class GetOrderListResponse
    {
        [JsonProperty(PropertyName ="")]
        public Result result { get; set; }
    }

}