using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Request
{
    public class GetOrderListRequest
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool detailedOrder { get; set; }
    }
}