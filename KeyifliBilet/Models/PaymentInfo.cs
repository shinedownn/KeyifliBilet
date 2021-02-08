using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Models
{
    public class PaymentInfo
    {
        public string cardNameSurname { get; set; }
        public string cardNumber { get; set; }
        public string cardMonth { get; set; }
        public string cardYear { get; set; }
        public string cardCcv { get; set; }

    }
}