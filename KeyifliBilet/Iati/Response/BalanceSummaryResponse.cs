using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class BalanceSummaryResult
    {
        public string currency { get; set; }
        public double balance { get; set; }
    }

    public class BalanceSummaryResponse
    {
        [JsonProperty(PropertyName = "result")]
        public BalanceSummaryResult result { get; set; }
    }
}