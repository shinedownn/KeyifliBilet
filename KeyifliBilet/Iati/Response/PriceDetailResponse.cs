using KeyifliBilet.Iati.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class PriceDetailBaggageAllowance
    {
        public int amount { get; set; }
        public string type { get; set; }
        public string alternativeType { get; set; }
        public string flightNumber { get; set; }
        public string departureAirport { get; set; }
        public string arrivalAirport { get; set; }
        public string carrier { get; set; }
        public string classCode { get; set; }
    }

    public class PriceDetailFare
    {
        public double total { get; set; }
        public double baseFare { get; set; }
        public double tax { get; set; }
        public double serviceFee { get; set; }
        public double supplement { get; set; }
        public double agencyCommission { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PessengerType passangerType { get; set; }
        [JsonProperty(PropertyName = "baggageAllowances")]
        public IList<PriceDetailBaggageAllowance> baggageAllowances { get; set; }
        public int passengerCount { get; set; }
        public bool needMilesNumber { get; set; }
        public bool needIdentity { get; set; }

    }

    public class PriceDetailResult
    {
        public string id { get; set; }
        public string searchId { get; set; }
        public string fareSearchKey { get; set; }
        public string currency { get; set; }
        public double totalPrice { get; set; }
        [JsonProperty(PropertyName = "fares")]
        public IList<PriceDetailFare> fares { get; set; }
        public string originalCurrency { get; set; }
        public double originalTotalPrice { get; set; }
        public double currencyConversionRate { get; set; }
        public double recommendedExtra { get; set; }
        public string identityType { get; set; }
        public bool cip { get; set; }
        public bool nonRefundable { get; set; }
        public bool multiProvider { get; set; }
        public bool classChanged { get; set; }
        public bool canBook { get; set; }
        public bool canReserve { get; set; }
    }

    public class PriceDetailResponse
    {
        [JsonProperty(PropertyName = "result")]
        public PriceDetailResult result { get; set; }
    }
}