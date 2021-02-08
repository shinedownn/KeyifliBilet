using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati_Models.Search
{
    public class Search
    {
        //public bool IsOneWay { get; set; }
        //public string DEP_Airport_Code { get; set; }
        //public string DEP_AirportName { get; set; }
        //public string DEP_CityName { get; set; }
        //public bool DEP_Airport_IsCity { get; set; } // allinFromCity
        //public string DepartureDate { get; set; }
        //public string ARR_Airport_Code { get; set; }
        //public string ARR_Airport_Name { get; set; }
        //public string ARR_Airport_CityName { get; set; }
        //public bool ARR_Airport_IsCity { get; set; }
        //public string ReturnDate { get; set; }
        //public int Adult { get; set; }
        //public int Child { get; set; }
        //public int Infant { get; set; }
        //public int Total { get; set; }
        //public string CabinClass { get; set; }
        //public bool NoStop { get; set; }


        public string fromAirport { get; set; }
        public bool allinFromCity { get; set; }
        public string toAirport { get; set; }
        public string fromDate { get; set; }
        public string returnDate { get; set; }
        public int adult { get; set; }
        public bool cip { get; set; }
        public int child { get; set; }
        public int infant { get; set; }
        public bool usePersonFares { get; set; }
        public bool getBaggageInfo { get; set; }
        public string currency { get; set; }
        public string classType { get; set; }
       // public string ﬁlterProvider { get; set; }

    }
}