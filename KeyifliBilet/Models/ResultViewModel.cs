using KeyifliBilet.Iati_Models.Result;
using KeyifliBilet.Iati_Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Models
{
    public class ResultViewModel
    {
        public List<Result> Result { get; set; }
        public ViewSearch Search { get; set; }
        public List<Route> WeekSearch { get; set; }
        public DestinationSearch DestinationSearch { get; set; }
    }
}