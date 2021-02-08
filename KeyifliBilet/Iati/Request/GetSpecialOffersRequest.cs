using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyifliBilet.Iati.Enums;

namespace KeyifliBilet.Iati.Request
{
    public class GetSpecialOffersRequest
    {
        public Currency? currency { get; set; }
    }
}