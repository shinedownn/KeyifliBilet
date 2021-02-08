using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class GetProviderRestrictionsResponseADULT
    {
        public string providerKey { get; set; }
        public string passengerType { get; set; }
        public bool canFlyAlone { get; set; }
        public int minimumAge { get; set; }
        public int maximumAge { get; set; }
        public bool allowEmpty { get; set; }
    }

    public class GetProviderRestrictionsResponseCHILD
    {
        public string providerKey { get; set; }
        public string passengerType { get; set; }
        public bool canFlyAlone { get; set; }
        public int minimumAge { get; set; }
        public int maximumAge { get; set; }
        public bool allowEmpty { get; set; }
    }

    public class GetProviderRestrictionsResponseINFANT
    {
        public string providerKey { get; set; }
        public string passengerType { get; set; }
        public bool canFlyAlone { get; set; }
        public int minimumAge { get; set; }
        public int maximumAge { get; set; }
        public bool allowEmpty { get; set; }
    }

    public class GetProviderRestrictionsResponseSENIOR
    {
        public string providerKey { get; set; }
        public string passengerType { get; set; }
        public bool canFlyAlone { get; set; }
        public int minimumAge { get; set; }
        public int maximumAge { get; set; }
        public bool allowEmpty { get; set; }
    }

    public class GetProviderRestrictionsResponseSTUDENT
    {
        public string providerKey { get; set; }
        public string passengerType { get; set; }
        public bool canFlyAlone { get; set; }
        public int minimumAge { get; set; }
        public int maximumAge { get; set; }
        public bool allowEmpty { get; set; }
    }

    public class GetProviderRestrictionsResponseMILITARY
    {
        public string providerKey { get; set; }
        public string passengerType { get; set; }
        public bool canFlyAlone { get; set; }
        public int minimumAge { get; set; }
        public int maximumAge { get; set; }
        public bool allowEmpty { get; set; }
    }

    public class GetProviderRestrictionsResponseResult
    {
        [JsonProperty(PropertyName = "ADULT")]
        public GetProviderRestrictionsResponseADULT ADULT { get; set; }
        [JsonProperty(PropertyName = "CHILD")]
        public GetProviderRestrictionsResponseCHILD CHILD { get; set; }
        [JsonProperty(PropertyName = "INFANT")]
        public GetProviderRestrictionsResponseINFANT INFANT { get; set; }
        [JsonProperty(PropertyName = "SENIOR")]
        public GetProviderRestrictionsResponseSENIOR SENIOR { get; set; }
        [JsonProperty(PropertyName = "STUDENT")]
        public GetProviderRestrictionsResponseSTUDENT STUDENT { get; set; }
        [JsonProperty(PropertyName = "MILITARY")]
        public GetProviderRestrictionsResponseMILITARY MILITARY { get; set; }
    }

    public class GetProviderRestrictionsResponse
    {
        [JsonProperty(PropertyName = "result")]
        public Result result { get; set; }
    }
}