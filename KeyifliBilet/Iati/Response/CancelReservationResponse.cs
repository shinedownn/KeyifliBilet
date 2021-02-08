using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class CancelReservationResponseResult
    {
        public bool voided { get; set; }
        public string description { get; set; }
    }

    public class CancelReservationResponse
    {
        [JsonProperty(PropertyName ="result")]
        public CancelReservationResponseResult result { get; set; }
    }
}