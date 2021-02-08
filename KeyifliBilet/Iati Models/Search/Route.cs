using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati_Models.Search
{
    public class Route
    {
        public string fromAirportCode { get; set; }
        public bool fromAirportIsCity { get; set; }
        public string toAirportCode { get; set; }
        public bool toAirportIsCity { get; set; }
    }

    public class RootObject
    {
        public List<Route> routes { get; set; }
        public string departureDateTimeBegin { get; set; }
        public string departureDateTimeEnd { get; set; }
        public string returnDateTimeBegin { get; set; }
        public string returnDateTimeEnd { get; set; }
        public string currency { get; set; }
    }
}