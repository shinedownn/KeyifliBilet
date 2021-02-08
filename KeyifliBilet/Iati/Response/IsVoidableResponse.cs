using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class IsVoidableResponseResult
    {
        public bool voidable { get; set; }
    }

    public class IsVoidableResponse
    {
        [JsonProperty(PropertyName ="result")]
        public IsVoidableResponseResult result { get; set; }
    }
}