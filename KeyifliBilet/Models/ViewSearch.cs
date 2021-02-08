using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Models
{
    public class ViewSearch
    {
        public bool IsOneWay { get; set; }
        public string DEP_Airport_Code { get; set; }
        public string DEP_Airport_Name { get; set; }
        public string DEP_Airport_CityName { get; set; }
        public bool DEP_Airport_IsCity { get; set; } // allinFromCity
        public string DepartureDate { get; set; }
        public string ARR_Airport_Code { get; set; }
        public string ARR_Airport_Name { get; set; }
        public string ARR_Airport_CityName { get; set; }
        public bool ARR_Airport_IsCity { get; set; }
        public string ReturnDate { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }
        public int Total { get; set; }
        public string CabinClass { get; set; }
        public bool NoStop { get; set; }
    }
}