using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Request
{
    public class CancelReservationRequest
    {
        public string pnr { get; set; }
        public int orderId { get; set; }
    }
}