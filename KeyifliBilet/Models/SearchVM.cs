using KeyifliBilet.Iati.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Models
{
    public class SearchVM
    {
        public ViewSearch Search { get; set; }
        public FlightResponse Flight { get; set; }
        public SearchVM()
        {
            Search = new ViewSearch();
            Flight = new FlightResponse();
        }
    }
}