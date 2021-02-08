using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class CurrencyListResponseCurrency
    {
        public string fromCurrency { get; set; }
        public string toCurrency { get; set; }
        public double rate { get; set; }
    }

    public class CurrencyListResult
    {
        [JsonProperty(PropertyName = "currencies")]
        public IList<CurrencyListResponseCurrency> currencies { get; set; }
    }

    public class CurrencyListResponse
    {
        [JsonProperty(PropertyName = "result")]
        public Result result { get; set; }
    }
}