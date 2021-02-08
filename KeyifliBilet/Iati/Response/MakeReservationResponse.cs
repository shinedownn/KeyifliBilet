using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class MakeReservationResult
    {
        public string referenceId { get; set; }
        public DateTime dueTo { get; set; }
        public int orderId { get; set; }
        public int price { get; set; }
        public IList<string> vendorRemarks { get; set; }
    }

    public class MakeReservationResponse
    {
        [JsonProperty(PropertyName ="result")]
        public MakeReservationResult result { get; set; }
    }

}