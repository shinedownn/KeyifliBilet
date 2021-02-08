using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class MultiLegPriceDetailResponse
    {
        public string id { get; set; }
        public string searchId { get; set; }
        public string fareSearchKey { get; set; }
        public string currency { get; set; }
        public double totalPrice { get; set; }
        [JsonProperty(PropertyName ="fares")]
        public IList<MultiLegPriceDetailResponseFare> fares { get; set; }
        public string originalCurrency { get; set; }
        public double originalTotalPrice { get; set; }
        public int currencyConversionRate { get; set; }
        public int recommendedExtra { get; set; }
        public bool canBook { get; set; }
        public bool canReserve { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.IdentityType identityType { get; set; }
        public bool cip { get; set; }
        public bool multiProvider { get; set; }
        public bool classChanged { get; set; }
        public bool nonRefundable { get; set; }
    }
    public class MultiLegPriceDetailResponseBaggageAllowance
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

    public class MultiLegPriceDetailResponseFare
    {
        public double total { get; set; }
        public int baseFare { get; set; }
        public double tax { get; set; }
        public double serviceFee { get; set; }
        public int supplement { get; set; }
        public int agencyCommission { get; set; }
        public string passangerType { get; set; }
        public int passengerCount { get; set; }
        public bool needMilesNumber { get; set; }
        public bool needIdentity { get; set; }
        [JsonProperty(PropertyName = "baggageAllowances")]
        public IList<MultiLegPriceDetailResponseBaggageAllowance> baggageAllowances { get; set; }
    }
}