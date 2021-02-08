using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Models
{
    public class TicketFare
    {        
        public string searchId { get; set; }
        public string flightId { get; set; }
        public string fareType { get; set; }
        public string flightType { get; set; }
        public string pricingType { get; set; }
        public string packageId { get; set; }
        public string providerKey { get; set; }
    }
}