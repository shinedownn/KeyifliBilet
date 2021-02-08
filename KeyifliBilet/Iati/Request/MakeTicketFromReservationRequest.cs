using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Request
{
    public class MakeTicketFromReservationRequest
    {
        public long orderId { get; set; }
        public bool cip { get; set; }
    }
}