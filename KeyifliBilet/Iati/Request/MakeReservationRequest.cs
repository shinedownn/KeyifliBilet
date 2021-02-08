using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class MakeReservationRequest
    {
        public string priceDetailId { get; set; }
        public string cip { get; set; }
        [JsonProperty(PropertyName="contactInfo")]
        public MakeReservationContactInfo contactInfo { get; set; }
        [JsonProperty(PropertyName = "passengers")]
        public IList<MakeReservationPassenger> passengers { get; set; }
    }

    public class MakeReservationPassenger
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PessengerType type { get; set; }
        public int count { get; set; }
    }
    public class MakeReservationContactInfo
    {
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
    }
}