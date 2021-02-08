using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class VoidPnrResponseResult
    {
        public bool voided { get; set; }
        public string description { get; set; }
    }

    public class VoidPnrResponse
    {
        [JsonProperty(PropertyName ="result")]
        public VoidPnrResponse result { get; set; }
    }
}