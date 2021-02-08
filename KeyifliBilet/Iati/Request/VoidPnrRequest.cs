using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Request
{
    public class VoidPnrRequest
    {
        public string pnr { get; set; }
        public long orderId { get; set; }
    }
}