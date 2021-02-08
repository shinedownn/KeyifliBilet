using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Request
{
    public class GetCalendarRequest
    {
        [JsonProperty(PropertyName ="routes")]
        public IList<GetCalendar> routes { get; set; }
        public string departureDateTimeBegin { get; set; }
        public string departureDateTimeEnd { get; set; }
        public string returnDateTimeBegin { get; set; }
        public string returnDateTimeEnd { get; set; }
        public string currency { get; set; }
    }

    public class GetCalendar
    {
        public string fromAirportCode { get; set; }
        public bool fromAirportIsCity { get; set; }
        public string toAirportCode { get; set; }
        public bool toAirportIsCity { get; set; }
    }
}