using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Iati_Models.Result
{
    public class Leg
    {
        public string flightNo { get; set; }
        public string aircraft { get; set; }
        public string carrierCode { get; set; }
        public string carrierName { get; set; }
        public string operatorCode { get; set; }
        public string operatorName { get; set; }
        public string validatingCarrierCode { get; set; }
        public string departureAirport { get; set; }
        public string localDepartureDate { get; set; }
        public DateTime departureTime { get; set; }
        public string departureAirportName { get; set; }
        public string departureCityName { get; set; }
        public string arrivalAirport { get; set; }
        public string localArrivalDate { get; set; }
        public DateTime arrivalTime { get; set; }
        public string arrivalAirportName { get; set; }
        public string arrivalCityName { get; set; }
        public int legDurationMinute { get; set; }
        public int waitTimeBeforeNextLeg { get; set; }
    }

    public class PersonFare
    {
        public double total { get; set; }
        public double baseFare { get; set; }
        public double tax { get; set; }
        public double serviceFee { get; set; }
        public double supplement { get; set; }
        public double agencyCommission { get; set; }
        public string passangerType { get; set; }
    }

    public class Detail
    {
        public double price { get; set; }
        public double serviceFee { get; set; }
        public double tax { get; set; }
        public double suplement { get; set; }
    }

    public class Fare
    {
        public double totalSingleAdultFare { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public List<PersonFare> personFares { get; set; }
        public List<string> segmentNames { get; set; }
        public int freeSeatCount { get; set; }
        public Detail detail { get; set; }
    }

    public class TestInfo
    {
        public bool suitableForTest { get; set; }
    }

    public class Flight
    {
        public string id { get; set; }
        public string providerKey { get; set; }
        public string pricingType { get; set; }
        public int packageId { get; set; }
        public List<Leg> legs { get; set; }
        public List<Fare> fares { get; set; }
        public int segmentCount { get; set; }
        public int baggage { get; set; }
        public int flightTimeHour { get; set; }
        public int flightTimeMinute { get; set; }
        public double layoverTime { get; set; }
        public bool hasCip { get; set; }
        public bool dayCross { get; set; }
        public bool returnFlight { get; set; }
        public TestInfo testInfo { get; set; }
    }

    public class Result
    {
        public string searchId { get; set; }
        public List<Flight> flights { get; set; }
    }

    public class RootObject
    {
        public Result result { get; set; }
    }
}