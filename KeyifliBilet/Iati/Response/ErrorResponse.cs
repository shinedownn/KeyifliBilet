using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati.Response
{
    public class Error
    {
        public string errorCode { get; set; }
        public string errorTitle { get; set; }
        public string description { get; set; }
        public string details { get; set; }
    }

    public class ErrorResponse
    {
        public Error error { get; set; }
    }

}