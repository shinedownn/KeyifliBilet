using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class GetChangedOrdersResponseOrijinalLeg
    {
        public string state { get; set; }
        public string departureAirport { get; set; }
        public string departureAirportName { get; set; }
        public string arrivalAirport { get; set; }
        public string arrivalAirportName { get; set; }
        public string flightNo { get; set; }
        public string classCode { get; set; }
        public string departureDate { get; set; }
        public string arrivalDate { get; set; }
        public string status { get; set; }
    }

    public class GetChangedOrdersResponseChangedLeg
    {
        public string state { get; set; }
        public string departureAirport { get; set; }
        public string departureAirportName { get; set; }
        public string arrivalAirport { get; set; }
        public string arrivalAirportName { get; set; }
        public string flightNo { get; set; }
        public string classCode { get; set; }
        public string departureDate { get; set; }
        public string arrivalDate { get; set; }
        public string status { get; set; }
    }

    public class GetChangedOrdersResponseLeg
    {
        [JsonProperty(PropertyName = "orijinalLeg")]
        public GetChangedOrdersResponseOrijinalLeg orijinalLeg { get; set; }
        [JsonProperty(PropertyName = "changedLeg")]
        public GetChangedOrdersResponseChangedLeg changedLeg { get; set; }
    }

    public class GetChangedOrdersResponseChangedOrder
    {
        public int changedId { get; set; }
        public int orderId { get; set; }
        public string pnr { get; set; }
        [JsonProperty(PropertyName = "legs")]
        public IList<GetChangedOrdersResponseLeg> legs { get; set; }
    }

    public class GetChangedOrdersResponseResult
    {
        [JsonProperty(PropertyName = "changedOrders")]
        public IList<GetChangedOrdersResponseChangedOrder> changedOrders { get; set; }
    }

    public class GetChangedOrdersResponse
    {
        [JsonProperty(PropertyName = "result")]
        public GetChangedOrdersResponseResult result { get; set; }
    }
}