using KeyifliBilet.Areas.Yonetim.Models;
using KeyifliBilet.Iati.Enums;
using KeyifliBilet.Iati.Request;
using KeyifliBilet.Iati.Response;
using KeyifliBilet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KeyifliBilet.Controllers
{
    public class FlightController : Controller
    {
        static FlightResponse list;
        //static List<Route> data;
        static List<Iati.Response.FlightResponse> selectedflights;
        static ViewSearch ViewSearch;
        KeyifliBiletEntities entities = new KeyifliBiletEntities();
        IndexViewModel vm = new IndexViewModel();
        public ActionResult RT()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Result(ViewSearch model)
        {
            var bt = System.Convert.ToDateTime(Convert.ToDateTime(model.DepartureDate).ToString("yyyy-MM-dd 00:00:00.000"));
            var kt = System.Convert.ToDateTime(Convert.ToDateTime(model.ReturnDate).ToString("yyyy-MM-dd 00:00:00.000"));

            decimal? agencyCommission = 0;

            var extraservicedep = entities.EkstraServis.Where(
                x => x.BaslangicTarihi <= bt &&
                x.BitisTarihi >= kt &&
                x.KalkisKod == model.DEP_Airport_Code &&
                x.VarisKod == model.ARR_Airport_Code
                ).Select(c => c).ToList().OrderByDescending(x => x.Id).FirstOrDefault();
            var promo = entities.Siniflar.Where(x => x.SinifAdi == "PROMO").Select(w => w.ServisBedeli).FirstOrDefault();
            var economy = entities.Siniflar.Where(x => x.SinifAdi == "ECONOMY").Select(w => w.ServisBedeli).FirstOrDefault();
            var business = entities.Siniflar.Where(x => x.SinifAdi == "BUSINESS").Select(w => w.ServisBedeli).FirstOrDefault();

            selectedflights = new List<FlightResponse>();
            ViewSearch = model;

            if (Convert.ToDateTime(model.DepartureDate) > Convert.ToDateTime(model.ReturnDate))
            {
                model.ReturnDate = model.DepartureDate;
            }
            var flight = new Iati.Request.FlightRequest()
            {
                adult = model.Adult.ToString(),
                allinFromCity = model.DEP_Airport_IsCity,
                allinToCity = model.ARR_Airport_IsCity,
                child = model.Child.ToString(),
                cip = false,
                classType = model.CabinClass == "ALL" ? null : (Iati.Enums.ClassType?)Enum.Parse(typeof(Iati.Enums.ClassType), model.CabinClass),
                currency = Iati.Enums.Currency.TL,
                fromAirport = model.DEP_Airport_Code,
                fromDate = Convert.ToDateTime(model.DepartureDate).ToString("yyyy-MM-dd"),
                getBaggageInfo = false,
                infant = model.Infant.ToString(),
                returnDate = model.IsOneWay ? "" : Convert.ToDateTime(model.ReturnDate).ToString("yyyy-MM-dd"),
                toAirport = model.ARR_Airport_Code,
                usePersonFares = true
            };
            var flights = await Iati.Search.GetFlights(flight);
            //test
            //  var data = flights.result.flights.Where(w => w.testInfo.suitableForTest == true && w.fares.Where(f => f.freeSeatCount > 0).Count() >= 0).ToList();
            //prod

            var data = flights.result.flights.ToList();
            foreach (var ff in selectedflights)
            {
                foreach (var item in ff.result.flights)
                {
                    foreach (var fa in item.fares)
                    {
                        foreach (var pf in fa.personFares)
                        {
                            if (extraservicedep == null)
                            {
                                if (fa.type == "PROMO")
                                {
                                    pf.agencyCommission = Convert.ToDouble(promo);
                                }
                                if (fa.type == "ECONOMY")
                                {
                                    pf.agencyCommission = Convert.ToDouble(economy);

                                }
                                if (fa.type == "BUSINESS")
                                {
                                    pf.agencyCommission = Convert.ToDouble(business);
                                }
                            }
                            else if (extraservicedep != null && fa.freeSeatCount <= extraservicedep.KoltukLimiti)
                            {
                                pf.agencyCommission = Convert.ToDouble(extraservicedep.ServisBedeli);
                            }
                            else
                            {
                                pf.agencyCommission = extraservicedep.ServisBedeli == null ? 0 : Convert.ToDouble(extraservicedep.ServisBedeli);
                            }
                        }
                    }
                }
            }

            foreach (var f in data.Where(x => x.providerKey == "Pegasus" && x.pricingType == "FREE_FORM").ToList())
            {

                foreach (var fare in f.fares)
                {
                    if (model.Infant == 0)
                    {
                        fare.personFares.ToList().Where(p => p.passangerType == "INFANT").ToList().ForEach(x => fare.personFares.Remove(x));
                    }
                    if (model.Adult == 0)
                    {
                        fare.personFares.ToList().Where(p => p.passangerType == "ADULT").ToList().ForEach(x => fare.personFares.Remove(x));
                    }
                    if (model.Child == 0)
                    {
                        fare.personFares.ToList().Where(p => p.passangerType == "CHILD").ToList().ForEach(x => fare.personFares.Remove(x));
                    }

                    foreach (var pf in fare.personFares)
                    {
                        //pf.agencyCommission = (pf.agencyCommission / 3) * (model.Adult + model.Child * model.Infant);
                        if (pf.passangerType == "INFANT")
                        {
                            pf.serviceFee = pf.serviceFee * model.Infant;
                            pf.supplement = pf.supplement * model.Infant;
                            pf.tax = pf.tax * model.Infant;
                            pf.total = pf.total * model.Infant;

                            pf.agencyCommission = pf.agencyCommission * model.Infant;

                        }
                        if (pf.passangerType == "ADULT")
                        {
                            pf.serviceFee = pf.serviceFee * model.Adult;
                            pf.supplement = pf.supplement * model.Adult;
                            pf.tax = pf.tax * model.Adult;
                            pf.total = pf.total * model.Adult;
                            pf.agencyCommission = pf.agencyCommission * model.Adult;
                        }
                        if (pf.passangerType == "CHILD")
                        {
                            pf.serviceFee = pf.serviceFee * model.Child;
                            pf.supplement = pf.supplement * model.Child;
                            pf.tax = pf.tax * model.Child;
                            pf.total = pf.total * model.Child;
                            pf.agencyCommission = pf.agencyCommission * model.Child;
                        }

                    }
                }
            }
            if (model.IsOneWay == false)
            {
                foreach (var d in data.Where(x => x.providerKey.Contains("Galileo") || x.providerKey.Contains("Amadeus") || x.providerKey.Contains("Sabre")).ToList())
                {
                    foreach (var fa in d.fares)
                    {
                        foreach (var pf in fa.personFares)
                        {
                            pf.total = pf.total / 2;
                            if (extraservicedep == null)
                            {
                                if (fa.type == "PROMO")
                                {
                                    pf.agencyCommission = Convert.ToDouble(promo);
                                }
                                if (fa.type == "ECONOMY")
                                {
                                    pf.agencyCommission = Convert.ToDouble(economy);

                                }
                                if (fa.type == "BUSINESS")
                                {
                                    pf.agencyCommission = Convert.ToDouble(business);
                                }
                            }
                            else if (extraservicedep != null && fa.freeSeatCount <= extraservicedep.KoltukLimiti)
                            {
                                pf.agencyCommission = Convert.ToDouble(extraservicedep.ServisBedeli);
                            }

                        }
                    }
                }
            }
            else
            {
                foreach (var d in data.Where(x => x.providerKey.Contains("Galileo") || x.providerKey.Contains("Amadeus") || x.providerKey.Contains("Sabre")).ToList())
                {
                    foreach (var fa in d.fares)
                    {
                        foreach (var pf in fa.personFares)
                        {

                            if (extraservicedep == null)
                            {
                                if (fa.type == "PROMO")
                                {
                                    pf.agencyCommission = Convert.ToDouble(promo);
                                }
                                if (fa.type == "ECONOMY")
                                {
                                    pf.agencyCommission = Convert.ToDouble(economy);

                                }
                                if (fa.type == "BUSINESS")
                                {
                                    pf.agencyCommission = Convert.ToDouble(business);
                                }
                            }
                            else if (extraservicedep != null && fa.freeSeatCount <= extraservicedep.KoltukLimiti)
                            {
                                pf.agencyCommission = Convert.ToDouble(extraservicedep.ServisBedeli);
                            }

                        }
                    }
                }
            }
            if (data.Where(x => x.providerKey != "Galileo" || x.providerKey != "Amadeus" || x.providerKey != "Sabre" || x.providerKey != "Pegasus").Count() > 0)
            {

                foreach (var item in data.Where(x => x.providerKey != "Galileo" || x.providerKey != "Amadeus" || x.providerKey != "Sabre" || x.providerKey != "Pegasus").ToList())
                {
                    foreach (var fa in item.fares)
                    {
                        foreach (var pf in fa.personFares)
                        {
                            if (extraservicedep == null)
                            {
                                if (fa.type == "PROMO")
                                {
                                    pf.agencyCommission = Convert.ToDouble(promo);
                                }
                                if (fa.type == "ECONOMY")
                                {
                                    pf.agencyCommission = Convert.ToDouble(economy);

                                }
                                if (fa.type == "BUSINESS")
                                {
                                    pf.agencyCommission = Convert.ToDouble(business);
                                }
                            }
                            else if (extraservicedep != null && fa.freeSeatCount <= extraservicedep.KoltukLimiti)
                            {
                                pf.agencyCommission = Convert.ToDouble(extraservicedep.ServisBedeli);
                            }
                        }
                    }
                }

            }



            list = new FlightResponse()
            {
                result = new Result()
                {
                    flights = data,
                    searchId = flights.result.searchId
                }
            };

            var vm = new SearchVM();
            vm.Flight = list;
            vm.Search = model;
            return View(vm);
        }

        [HttpPost]
        public ActionResult SelectFlight(TicketFare fare)
        {
            var ticketFare = new Iati_Models.Search.TicketFareSearch();
            ticketFare.flightId = fare.flightId;
            ticketFare.flightType = fare.flightType;
            ticketFare.fareType = fare.fareType;
            ticketFare.packageId = fare.packageId;
            ticketFare.pricingType = fare.pricingType;
            ticketFare.searchId = fare.searchId;
            ticketFare.providerKey = fare.providerKey;
            //var flights = list.FirstOrDefault(x => x.flights.FirstOrDefault(f => f.id == fare.flightId) != null).flights;
            var flight = list.result.flights.FirstOrDefault(x => x.id == fare.flightId);
            var f = flight.fares.Where(x => x.type == fare.fareType).ToList();

            if (ViewSearch.IsOneWay)
            {
                if (fare.flightType == "Go")
                {

                    selectedflights = new List<Iati.Response.FlightResponse>();
                    selectedflights.Add(new FlightResponse()
                    {
                        result = new Result()
                        {
                            searchId = fare.searchId,
                            flights = new List<Flight>
                            {
                                new Flight()
                                {
                                    baggage=flight.baggage,
                                    dayCross=flight.dayCross,
                                    fares=f,
                                    flightTimeHour=flight.flightTimeHour,
                                    flightTimeMinute=flight.flightTimeMinute,
                                    hasCip=flight.hasCip,
                                    id=flight.id,
                                    layoverTime=flight.layoverTime,
                                    legs=flight.legs,
                                    packageId=flight.packageId,
                                    pricingType=flight.pricingType,
                                    providerKey=flight.providerKey,
                                    returnFlight=flight.returnFlight,
                                    segmentCount=flight.segmentCount,
                                    testInfo=flight.testInfo

                                }
                            }
                        }
                    });
                    var bt = System.Convert.ToDateTime(Convert.ToDateTime(ViewSearch.DepartureDate).ToString("yyyy-MM-dd 00:00:00.000"));
                    var kt = System.Convert.ToDateTime(Convert.ToDateTime(ViewSearch.ReturnDate).ToString("yyyy-MM-dd 00:00:00.000"));

                    decimal? agencyCommission = 0;

                    var extraservicedep = entities.EkstraServis.Where(
                        x => x.BaslangicTarihi <= bt &&
                        x.BitisTarihi >= kt &&
                        x.KalkisKod == ViewSearch.DEP_Airport_Code &&
                        x.VarisKod == ViewSearch.ARR_Airport_Code
                        ).Select(c => c).ToList().OrderByDescending(x => x.Id).FirstOrDefault();
                    var promo = entities.Siniflar.Where(x => x.SinifAdi == "PROMO").Select(w => w.ServisBedeli).FirstOrDefault();
                    var economy = entities.Siniflar.Where(x => x.SinifAdi == "ECONOMY").Select(w => w.ServisBedeli).FirstOrDefault();
                    var business = entities.Siniflar.Where(x => x.SinifAdi == "BUSINESS").Select(w => w.ServisBedeli).FirstOrDefault();
                    foreach (var ff in selectedflights)
                    {
                        foreach (var item in ff.result.flights)
                        {
                            foreach (var fa in item.fares)
                            {
                                foreach (var pf in fa.personFares)
                                {
                                    if (extraservicedep == null)
                                    {
                                        if (fa.type == "PROMO")
                                        {
                                            pf.agencyCommission = Convert.ToDouble(promo);
                                        }
                                        if (fa.type == "ECONOMY")
                                        {
                                            pf.agencyCommission = Convert.ToDouble(economy);

                                        }
                                        if (fa.type == "BUSINESS")
                                        {
                                            pf.agencyCommission = Convert.ToDouble(business);
                                        }
                                    }
                                    else if (extraservicedep != null && fa.freeSeatCount <= extraservicedep.KoltukLimiti)
                                    {
                                        pf.agencyCommission = Convert.ToDouble(extraservicedep.ServisBedeli);
                                    }
                                    else
                                    {
                                        pf.agencyCommission = extraservicedep.ServisBedeli == null ? 0 : Convert.ToDouble(extraservicedep.ServisBedeli);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!ViewSearch.IsOneWay)
            {
                if (fare.flightType != "Go")
                {
                    if (selectedflights == null) selectedflights = new List<FlightResponse>();
                    if (selectedflights.Count() > 1)
                    {
                        selectedflights = new List<Iati.Response.FlightResponse>();
                    }

                    selectedflights.Add(new FlightResponse()
                    {
                        result = new Result()
                        {
                            searchId = fare.searchId,
                            flights = new List<Flight>
                            {
                                new Flight()
                                {
                                    baggage = flight.baggage,
                                    dayCross = flight.dayCross,
                                    fares = f,
                                    flightTimeHour = flight.flightTimeHour,
                                    flightTimeMinute = flight.flightTimeMinute,
                                    hasCip = flight.hasCip,
                                    id = flight.id,
                                    layoverTime = flight.layoverTime,
                                    legs = flight.legs,
                                    packageId = flight.packageId,
                                    pricingType = flight.pricingType,
                                    providerKey = flight.providerKey,
                                    returnFlight = flight.returnFlight,
                                    segmentCount = flight.segmentCount,
                                    testInfo = flight.testInfo

                                }
                            }
                        }
                    });
                    var bt = System.Convert.ToDateTime(Convert.ToDateTime(ViewSearch.DepartureDate).ToString("yyyy-MM-dd 00:00:00.000"));
                    var kt = System.Convert.ToDateTime(Convert.ToDateTime(ViewSearch.ReturnDate).ToString("yyyy-MM-dd 00:00:00.000"));
                    var extraservicedep = entities.EkstraServis.Where(
                        x => x.BaslangicTarihi <= bt &&
                        x.BitisTarihi >= kt &&
                        x.KalkisKod == ViewSearch.DEP_Airport_Code &&
                        x.VarisKod == ViewSearch.ARR_Airport_Code
                        ).Select(c => c).ToList().OrderByDescending(x => x.Id).FirstOrDefault();

                    var extraservicearr = entities.EkstraServis.Where(
                        x => x.BaslangicTarihi <= bt &&
                        x.BitisTarihi >= kt &&
                        x.KalkisKod == ViewSearch.ARR_Airport_Code &&
                        x.VarisKod == ViewSearch.DEP_Airport_Code
                        ).Select(c => c).ToList().OrderByDescending(x => x.Id).FirstOrDefault();


                    var promo = entities.Siniflar.Where(x => x.SinifAdi == "PROMO").Select(w => w.ServisBedeli).FirstOrDefault();
                    var economy = entities.Siniflar.Where(x => x.SinifAdi == "ECONOMY").Select(w => w.ServisBedeli).FirstOrDefault();
                    var business = entities.Siniflar.Where(x => x.SinifAdi == "BUSINESS").Select(w => w.ServisBedeli).FirstOrDefault();
                    foreach (var ff in selectedflights)
                    {
                        foreach (var item in ff.result.flights)
                        {
                            if (!item.returnFlight)
                            {
                                foreach (var fa in item.fares)
                                {


                                    foreach (var pf in fa.personFares)
                                    {
                                        if (extraservicedep == null)
                                        {
                                            if (fa.type == "PROMO")
                                            {
                                                pf.agencyCommission = Convert.ToDouble(promo);
                                            }
                                            if (fa.type == "ECONOMY")
                                            {
                                                pf.agencyCommission = Convert.ToDouble(economy);

                                            }
                                            if (fa.type == "BUSINESS")
                                            {
                                                pf.agencyCommission = Convert.ToDouble(business);
                                            }
                                        }
                                        else if (extraservicedep != null && fa.freeSeatCount <= extraservicedep.KoltukLimiti)
                                        {
                                            pf.agencyCommission = Convert.ToDouble(extraservicedep.ServisBedeli);
                                        }
                                        else
                                        {
                                            pf.agencyCommission = extraservicedep.ServisBedeli == null ? 0 : Convert.ToDouble(extraservicedep.ServisBedeli);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var fa in item.fares)
                                {


                                    foreach (var pf in fa.personFares)
                                    {
                                        if (extraservicearr == null)
                                        {
                                            if (fa.type == "PROMO")
                                            {
                                                pf.agencyCommission = Convert.ToDouble(promo);
                                            }
                                            if (fa.type == "ECONOMY")
                                            {
                                                pf.agencyCommission = Convert.ToDouble(economy);

                                            }
                                            if (fa.type == "BUSINESS")
                                            {
                                                pf.agencyCommission = Convert.ToDouble(business);
                                            }
                                        }
                                        else if (extraservicearr != null && fa.freeSeatCount <= extraservicearr.KoltukLimiti)
                                        {
                                            pf.agencyCommission = Convert.ToDouble(extraservicearr.ServisBedeli);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (selectedflights == null) selectedflights = new List<FlightResponse>();
                    if (selectedflights.Count() > 1)
                    {
                        selectedflights = new List<Iati.Response.FlightResponse>();
                    }
                    selectedflights.Add(new FlightResponse()
                    {
                        result = new Result()
                        {
                            searchId = fare.searchId,
                            flights = new List<Flight>
                            {
                                new Flight()
                                {
                                    baggage = flight.baggage,
                                    dayCross = flight.dayCross,
                                    fares = f,
                                    flightTimeHour = flight.flightTimeHour,
                                    flightTimeMinute = flight.flightTimeMinute,
                                    hasCip = flight.hasCip,
                                    id = flight.id,
                                    layoverTime = flight.layoverTime,
                                    legs = flight.legs,
                                    packageId = flight.packageId,
                                    pricingType = flight.pricingType,
                                    providerKey = flight.providerKey,
                                    returnFlight = flight.returnFlight,
                                    segmentCount = flight.segmentCount,
                                    testInfo = flight.testInfo

                                }
                            }
                        }
                    });

                }

            }
            ViewBag.ViewSearch = ViewSearch;
            return Json(flight, JsonRequestBehavior.AllowGet);

        }

        //[Route("{fromAirport}-{toAirport}")]
        //public ActionResult Result()
        //{
        //    ResultViewModel vm = new ResultViewModel();

        //    vm.Result = list;
        //    vm.Search = ViewSearch;
        //    return View(vm);
        //}
        // [Route("{fromAirport}-{toAirport}/odeme")]


        public async Task<ActionResult> Payment(string k)
        {
            List<PriceDetailFareReferences> frist = new List<PriceDetailFareReferences>();

            foreach (var result in selectedflights)
            {
                foreach (var flight in result.result.flights)
                {
                    var iid = await Iati.Search.GetItineraryDetail(new GetItineraryDetailRequest() { id = flight.id, currency = Currency.TL });
                    frist.Add(new PriceDetailFareReferences() { itineraryId = iid.result.id, fareType = (FareType)Enum.Parse(typeof(FareType), flight.fares[0].type) });

                }

            }
            var pid = await Iati.Search.GetPriceDetail(new PriceDetailRequest()
            {
                cip = false,
                currency = Currency.TL,
                searchId = selectedflights[0].result.searchId,
                fareRefereces = frist,
                passengers = new List<PriceDetailPassenger>() {
                    new PriceDetailPassenger(){ count=ViewSearch.Adult+ViewSearch.Child, type=PessengerType.ADULT }
                    }

            });


            double agencyCommission = 0;
            foreach (var item in selectedflights)
            {
                foreach (var flight in item.result.flights)
                {
                    foreach (var fare in flight.fares)
                    {
                        for (int i = 0; i < fare.personFares.Count; i++)
                        {
                            agencyCommission += fare.personFares[i].agencyCommission;
                        }
                    }
                }
            }
            pid.result.fares[0].agencyCommission = agencyCommission;


            //TC Doğrulama
            string domain = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            IsBankasiVm model = new IsBankasiVm();
            var total = Convert.ToDecimal(selectedflights.Sum(x => x.result.flights.Sum(y => y.fares.Sum(f => f.personFares.Sum(p => p.total)))));
            model.clientId = "700664827078";
            model.amount = total.ToString();
            model.oid = "";
            model.okUrl = domain + "/Flight/Sonuc"; //Bankasnın dönüş Urlsi
            model.failUrl = domain + "/Flight/Sonuc";
            model.rnd = DateTime.Now.ToString();
            model.storekey = "MGKR2414";
            model.storetype = "3D_PAY";

            String hashstr = model.clientId + model.oid + model.amount + model.okUrl + model.failUrl + model.rnd + model.storekey;
            System.Security.Cryptography.SHA1 sha = new
            System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(hashstr);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            model.hash = Convert.ToBase64String(inputbytes);
            ViewBag.PriceDetailResponse = pid;
            ViewBag.ViewSearch = ViewSearch;
            ViewBag.SelectedFlights = selectedflights;
            ViewBag.Siniflar = entities.Siniflar.ToList();

            return View(model);

        }
        //[HandleError(ExceptionType = typeof(TicketException), View = "TicketError")]
        public async Task<ActionResult> Sonuc(List<Ticket> passenger, PaymentInfo payment, ContactInfo contact)
        {
            var e = Request.Form.GetEnumerator();
            while (e.MoveNext())
            {
                String xkey = (String)e.Current;
                String xval = Request.Form.Get(xkey);
            }

            String hashparams = Request.Form.Get("HASHPARAMS");
            String hashparamsval = Request.Form.Get("HASHPARAMSVAL");
            String storekey = "MGKR2414"; //Sizin Storkey Adresiniz
            String paramsval = "";
            int index1 = 0, index2 = 0;
            // hash hesaplamada kullanılacak değerler ayrıştırılıp değerleri birleştiriliyor.
            do
            {
                index2 = hashparams.IndexOf(":", index1);
                String val = Request.Form.Get(hashparams.Substring(index1, index2 - index1)) == null ? "" : Request.Form.Get(hashparams.Substring(index1, index2 - index1));
                paramsval += val;
                index1 = index2 + 1;
            }
            while (index1 < hashparams.Length);

            //out.println("hashparams="+hashparams+"<br/>");
            //out.println("hashparamsval="+hashparamsval+"<br/>");
            //out.println("paramsval="+paramsval+"<br/>");
            String hashval = paramsval + storekey;         //elde edilecek hash değeri için paramsval e store key ekleniyor. (işyeri anahtarı)
            String hashparam = Request.Form.Get("HASH");

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(hashval);
            byte[] inputbytes = sha.ComputeHash(hashbytes);

            String hash = Convert.ToBase64String(inputbytes); //Güvenlik ve kontrol amaçlı oluşturulan hash
            if (!paramsval.Equals(hashparamsval) || !hash.Equals(hashparam)) //oluşturulan hash ile gelen hash ve hash parametreleri değerleri ile ayrıştırılıp edilen edilen aynı olmalı.
            {
               
            }
            // Ödeme için gerekli parametreler
            String nameval = "Emre";              //İşyeri kullanıcı adı
            String passwordval = "Emre1341?";        //İşyeri şifresi
            String clientidval = Request.Form.Get("clientid"); // İşyeri numarası
            String modeval = "P";                   //P olursa gerçek işlem, T olursa test işlemi yapar.
            String typeval = "Auth";           //Auth PreAuth PostAuth Credit Void olabilir.
            String expiresval = Request.Form.Get("Ecom_Payment_Card_ExpDate_Month") + "/" + Request.Form.Get("Ecom_Payment_Card_ExpDate_Year"); //Kredi Kartı son kullanım tarihi mm/yy formatından olmalı
            String cv2val = Request.Form.Get("cv2");    //Güvenlik Kodu
            String totalval = Request.Form.Get("amount"); //Tutar
            String numberval = Request.Form.Get("md");    //Kart numarası olarak 3d sonucu dönem md parametresi kullanılır.
            String taksitval = "";                  //Taksit sayısı peşin satışlar da boş olarak gönderilmelidir.
            String currencyval = "949";           //ytl için
            String orderidval = "";                //Sipariş numarası

            String mdErrorMsg = Request.Form.Get("mdErrorMsg");
            String mdstatus = Request.Form.Get("mdStatus"); // mdStatus 3d işlemin sonucu ile ilgili bilgi verir. 1,2,3,4 başarılı, 5,6,7,8,9,0 başarısızdır.
            if (mdstatus.Equals("1") || mdstatus.Equals("2") || mdstatus.Equals("3") || mdstatus.Equals("4")) //3D Onayı alınmıştır.
            {

              
                String cardholderpresentcodeval = "13";
                String payersecuritylevelval = Request.Form.Get("eci");
                String payertxnidval = Request.Form.Get("xid");
                String payerauthenticationcodeval = Request.Form.Get("cavv");
                String ipaddressval = "";
                String emailval = "";
                String groupidval = "";
                String transidval = "";
                String useridval = "";

                //Fatura Bilgileri
                String billnameval = "";      //Fatur İsmi
                String billstreet1val = "";   //Fatura adres 1
                String billstreet2val = "";   //Fatura adres 2
                String billstreet3val = "";   //Fatura adres 3
                String billcityval = "";      //Fatura şehir
                String billstateprovval = ""; //Fatura eyalet
                String billpostalcodeval = ""; //Fatura posta kodu

                //Teslimat Bilgileri
                String shipnameval = "";      //isim
                String shipstreet1val = "";   //adres 1
                String shipstreet2val = "";   //adres 2
                String shipstreet3val = "";   //adres 3
                String shipcityval = "";      //şehir
                String shipstateprovval = ""; //eyalet
                String shippostalcodeval = "";//posta kodu


                String extraval = "";



                //Ödeme için gerekli xml yapısı oluşturuluyor

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                System.Xml.XmlDeclaration dec =
                    doc.CreateXmlDeclaration("1.0", "ISO-8859-9", "yes");

                doc.AppendChild(dec);


                System.Xml.XmlElement cc5Request = doc.CreateElement("CC5Request");
                doc.AppendChild(cc5Request);

                System.Xml.XmlElement name = doc.CreateElement("Name");
                name.AppendChild(doc.CreateTextNode(nameval));
                cc5Request.AppendChild(name);

                System.Xml.XmlElement password = doc.CreateElement("Password");
                password.AppendChild(doc.CreateTextNode(passwordval));
                cc5Request.AppendChild(password);

                System.Xml.XmlElement clientid = doc.CreateElement("ClientId");
                clientid.AppendChild(doc.CreateTextNode(clientidval));
                cc5Request.AppendChild(clientid);

                System.Xml.XmlElement ipaddress = doc.CreateElement("IPAddress");
                ipaddress.AppendChild(doc.CreateTextNode(ipaddressval));
                cc5Request.AppendChild(ipaddress);

                System.Xml.XmlElement email = doc.CreateElement("Email");
                email.AppendChild(doc.CreateTextNode(emailval));
                cc5Request.AppendChild(email);

                System.Xml.XmlElement mode = doc.CreateElement("Mode");
                mode.AppendChild(doc.CreateTextNode(modeval));
                cc5Request.AppendChild(mode);

                System.Xml.XmlElement orderid = doc.CreateElement("OrderId");
                orderid.AppendChild(doc.CreateTextNode(orderidval));
                cc5Request.AppendChild(orderid);

                System.Xml.XmlElement groupid = doc.CreateElement("GroupId");
                groupid.AppendChild(doc.CreateTextNode(groupidval));
                cc5Request.AppendChild(groupid);

                System.Xml.XmlElement transid = doc.CreateElement("TransId");
                transid.AppendChild(doc.CreateTextNode(transidval));
                cc5Request.AppendChild(transid);

                System.Xml.XmlElement userid = doc.CreateElement("UserId");
                userid.AppendChild(doc.CreateTextNode(useridval));
                cc5Request.AppendChild(userid);

                System.Xml.XmlElement type = doc.CreateElement("Type");
                type.AppendChild(doc.CreateTextNode(typeval));
                cc5Request.AppendChild(type);

                System.Xml.XmlElement number = doc.CreateElement("Number");
                number.AppendChild(doc.CreateTextNode(numberval));
                cc5Request.AppendChild(number);

                System.Xml.XmlElement expires = doc.CreateElement("Expires");
                expires.AppendChild(doc.CreateTextNode(expiresval));
                cc5Request.AppendChild(expires);

                System.Xml.XmlElement cvv2val = doc.CreateElement("Cvv2Val");
                cvv2val.AppendChild(doc.CreateTextNode(cv2val));
                cc5Request.AppendChild(cvv2val);

                System.Xml.XmlElement total = doc.CreateElement("Total");
                total.AppendChild(doc.CreateTextNode(totalval));
                cc5Request.AppendChild(total);

                System.Xml.XmlElement currency = doc.CreateElement("Currency");
                currency.AppendChild(doc.CreateTextNode(currencyval));
                cc5Request.AppendChild(currency);

                System.Xml.XmlElement taksit = doc.CreateElement("Taksit");
                taksit.AppendChild(doc.CreateTextNode(taksitval));
                cc5Request.AppendChild(taksit);

                System.Xml.XmlElement payertxnid = doc.CreateElement("PayerTxnId");
                payertxnid.AppendChild(doc.CreateTextNode(payertxnidval));
                cc5Request.AppendChild(payertxnid);

                System.Xml.XmlElement payersecuritylevel = doc.CreateElement("PayerSecurityLevel");
                payersecuritylevel.AppendChild(doc.CreateTextNode(payersecuritylevelval));
                cc5Request.AppendChild(payersecuritylevel);

                System.Xml.XmlElement payerauthenticationcode = doc.CreateElement("PayerAuthenticationCode");
                payerauthenticationcode.AppendChild(doc.CreateTextNode(payerauthenticationcodeval));
                cc5Request.AppendChild(payerauthenticationcode);

                System.Xml.XmlElement cardholderpresentcode = doc.CreateElement("CardholderPresentCode");
                cardholderpresentcode.AppendChild(doc.CreateTextNode(cardholderpresentcodeval));
                cc5Request.AppendChild(cardholderpresentcode);

                System.Xml.XmlElement billto = doc.CreateElement("BillTo");
                cc5Request.AppendChild(billto);

                System.Xml.XmlElement billname = doc.CreateElement("Name");
                billname.AppendChild(doc.CreateTextNode(billnameval));
                billto.AppendChild(billname);

                System.Xml.XmlElement billstreet1 = doc.CreateElement("Street1");
                billstreet1.AppendChild(doc.CreateTextNode(billstreet1val));
                billto.AppendChild(billstreet1);

                System.Xml.XmlElement billstreet2 = doc.CreateElement("Street2");
                billstreet2.AppendChild(doc.CreateTextNode(billstreet2val));
                billto.AppendChild(billstreet2);

                System.Xml.XmlElement billstreet3 = doc.CreateElement("Street3");
                billstreet3.AppendChild(doc.CreateTextNode(billstreet3val));
                billto.AppendChild(billstreet3);

                System.Xml.XmlElement billcity = doc.CreateElement("City");
                billcity.AppendChild(doc.CreateTextNode(billcityval));
                billto.AppendChild(billcity);

                System.Xml.XmlElement billstateprov = doc.CreateElement("StateProv");
                billstateprov.AppendChild(doc.CreateTextNode(billstateprovval));
                billto.AppendChild(billstateprov);

                System.Xml.XmlElement billpostalcode = doc.CreateElement("PostalCode");
                billpostalcode.AppendChild(doc.CreateTextNode(billpostalcodeval));
                billto.AppendChild(billpostalcode);



                System.Xml.XmlElement shipto = doc.CreateElement("ShipTo");
                cc5Request.AppendChild(shipto);

                System.Xml.XmlElement shipname = doc.CreateElement("Name");
                shipname.AppendChild(doc.CreateTextNode(shipnameval));
                shipto.AppendChild(shipname);

                System.Xml.XmlElement shipstreet1 = doc.CreateElement("Street1");
                shipstreet1.AppendChild(doc.CreateTextNode(shipstreet1val));
                shipto.AppendChild(shipstreet1);

                System.Xml.XmlElement shipstreet2 = doc.CreateElement("Street2");
                shipstreet2.AppendChild(doc.CreateTextNode(shipstreet2val));
                shipto.AppendChild(shipstreet2);

                System.Xml.XmlElement shipstreet3 = doc.CreateElement("Street3");
                shipstreet3.AppendChild(doc.CreateTextNode(shipstreet3val));
                shipto.AppendChild(shipstreet3);

                System.Xml.XmlElement shipcity = doc.CreateElement("City");
                shipcity.AppendChild(doc.CreateTextNode(shipcityval));
                shipto.AppendChild(shipcity);

                System.Xml.XmlElement shipstateprov = doc.CreateElement("StateProv");
                shipstateprov.AppendChild(doc.CreateTextNode(shipstateprovval));
                shipto.AppendChild(shipstateprov);

                System.Xml.XmlElement shippostalcode = doc.CreateElement("PostalCode");
                shippostalcode.AppendChild(doc.CreateTextNode(shippostalcodeval));
                shipto.AppendChild(shippostalcode);


                System.Xml.XmlElement extra = doc.CreateElement("Extra");
                extra.AppendChild(doc.CreateTextNode(extraval));
                cc5Request.AppendChild(extra);
                String xmlval = doc.OuterXml;     //Oluşturulan xml string olarak alınıyor.
                                                  // Ödeme için bağlantı kuruluyor. ve post ediliyor
                String url = "https://sanalpos.isbank.com.tr/fim/api";
                System.Net.HttpWebResponse resp = null;
                try
                {
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);

                    string postdata = "DATA=" + xmlval.ToString();
                    byte[] postdatabytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(postdata);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = postdatabytes.Length;
                    System.IO.Stream requeststream = request.GetRequestStream();
                    requeststream.Write(postdatabytes, 0, postdatabytes.Length);
                    requeststream.Close();

                    resp = (System.Net.HttpWebResponse)request.GetResponse();
                    System.IO.StreamReader responsereader = new System.IO.StreamReader(resp.GetResponseStream(), System.Text.Encoding.GetEncoding("ISO-8859-9"));
                    String gelenXml = responsereader.ReadToEnd(); //Gelen xml string olarak alındı.


                    System.Xml.XmlDocument gelen = new System.Xml.XmlDocument();
                    gelen.LoadXml(gelenXml);    //string xml dökumanına çevrildi.

                    System.Xml.XmlNodeList list = gelen.GetElementsByTagName("Response");
                    String xmlResponse = list[0].InnerText;
                    list = gelen.GetElementsByTagName("AuthCode");
                    String xmlAuthCode = list[0].InnerText;
                    list = gelen.GetElementsByTagName("HostRefNum");
                    String xmlHostRefNum = list[0].InnerText;
                    list = gelen.GetElementsByTagName("ProcReturnCode");
                    String xmlProcReturnCode = list[0].InnerText;
                    list = gelen.GetElementsByTagName("TransId");
                    String xmlTransId = list[0].InnerText;
                    list = gelen.GetElementsByTagName("ErrMsg");
                    String xmlErrMsg = list[0].InnerText;
                    if ("Approved".Equals(xmlResponse))
                    {
                        try
                        {
                            List<TicketWithTCNOPassenger> list2 = new List<TicketWithTCNOPassenger>();
                            List<PriceDetailFareReferences> frist = new List<PriceDetailFareReferences>();
                            foreach (var ticket in passenger)
                            {
                                var t = new TicketWithTCNOPassenger();

                                t.birthdate = ticket.birthdate.ToString("yyyy-MM-dd");
                                t.gender = (Iati.Enums.Gender)Enum.Parse(typeof(Gender), ticket.gender);
                                t.name = ticket.name;
                                t.surname = ticket.surname;
                                t.type = PessengerType.ADULT;
                                t.identityNo = ticket.IsTurkish ? ticket.IdentityNo : "";
                                t.turkishCitizen = ticket.IsTurkish.ToString();
                                list2.Add(t);
                            }
                            foreach (var result in selectedflights)
                            {
                                foreach (var flight in result.result.flights)
                                {
                                    var iid = await Iati.Search.GetItineraryDetail(new GetItineraryDetailRequest() { id = flight.id, currency = Currency.TL });
                                    frist.Add(new PriceDetailFareReferences() { itineraryId = iid.result.id, fareType = (FareType)Enum.Parse(typeof(FareType), flight.fares[0].type) });
                                }
                            }
                            var pid = await Iati.Search.GetPriceDetail(new PriceDetailRequest()
                            {
                                cip = false,
                                currency = Currency.TL,
                                searchId = selectedflights[0].result.searchId,
                                fareRefereces = frist,
                                passengers = new List<PriceDetailPassenger>() {
                    new PriceDetailPassenger(){ count=passenger.Count, type=PessengerType.ADULT }
                    }

                            });
                            var tickets = new Iati.Request.MakeTicketWithTCNORequest()
                            {
                                passengers = list2,
                                contactInfo = new TicketwithTCNOContactInfo() { email = contact.email, mobilePhoneNumber = contact.mobilePhoneNumber, phoneNumber = contact.phoneNumber },
                                priceDetailId = pid.result.id,
                                cip = "false",
                                notes = ""
                            };
                            try
                            {
                                var res = await Iati.Search.MakeTicketWithTcNo(tickets);
                                if (res.result != null)
                                {


                                    var order = await Iati.Search.GetOrder(new GetOrderRequest { orderId = res.result.orderId });
                                    //Veritabanı Kaydı
                                    OrderResult getorder = new OrderResult();

                                    getorder.contactEmail = order.result.contactEmail;
                                    getorder.contactGsm = order.result.contactGsm;
                                    getorder.creationDate = order.result.creationDate;
                                    getorder.note = order.result.note;
                                    getorder.provider = order.result.provider;
                                    getorder.status = order.result.status;
                                    getorder.pax = order.result.pax;
                                    getorder.orderId = order.result.orderId;


                                    if (Session["UyeId"] != null)
                                    {
                                        int sesion = Convert.ToInt32(Session["UyeId"]);
                                        getorder.UyeId = sesion;
                                    }
                                    else
                                    {
                                        getorder.UyeId = null;
                                    }
                                    entities.OrderResult.Add(getorder);
                                    entities.SaveChanges();
                                    var id = entities.OrderResult.FirstOrDefault(w => w.orderId == getorder.orderId).Id;

                                    foreach (var item in selectedflights)
                                    {
                                        foreach (var data in item.result.flights)
                                        {
                                            foreach (var rrr in data.legs)
                                            {

                                                legs leg = new legs();
                                                leg.aircraft = rrr.aircraft;
                                                leg.arrivalAirport = rrr.arrivalAirport;
                                                leg.arrivalAirportName = rrr.arrivalAirportName;
                                                leg.arrivalCityName = rrr.arrivalCityName;
                                                leg.arrivalTime = rrr.arrivalTime;
                                                leg.departureAirport = rrr.departureAirport;
                                                leg.departureAirportName = rrr.departureAirportName;
                                                leg.departureCityName = rrr.departureCityName;
                                                leg.departureTime = rrr.departureTime;
                                                leg.flightNo = rrr.flightNo;
                                                leg.Id = id;
                                                leg.localArrivalDate = rrr.localArrivalDate;
                                                leg.localDepartureDate = rrr.localDepartureDate;
                                                leg.operatorName = rrr.operatorName;
                                                leg.returnFlight = data.returnFlight;
                                                entities.legs.Add(leg);
                                                entities.SaveChanges();
                                            }
                                        }
                                    }
                                    BookingInfo book = new BookingInfo();
                                    book.Id = id;
                                    book.pnr = order.result.bookingInfo.pnr;

                                    entities.BookingInfo.Add(book);
                                    entities.SaveChanges();
                                    foreach (var item in order.result.passengers)
                                    {
                                        passengers pas = new passengers();
                                        pas.Id = id;
                                        pas.agencyComission = item.agencyComission;
                                        pas.birthdate = item.birthdate;
                                        pas.city = item.city;
                                        pas.country = item.country;
                                        pas.currency = item.currency;
                                        pas.eticket = item.eticket;
                                        pas.name = item.name;
                                        pas.persontype = item.persontype;
                                        pas.serviceFee = item.serviceFee.ToString();
                                        pas.surname = item.surname;
                                        pas.tax = item.tax.ToString();
                                        pas.totalPrice = item.totalPrice.ToString();

                                        entities.passengers.Add(pas);
                                        entities.SaveChanges();

                                    }
                                    var orderrr = entities.OrderResult.FirstOrDefault(w => w.Id == id);
                                    string html = "";
                                    html += "<div class=''>                                                                                                                                                                                                                             ";
                                    html += "	<div class='aHl'></div>                                                                                                                                                                                                                 ";
                                    html += "	<div id=':19n' tabindex='-1'></div>                                                                                                                                                                                                     ";
                                    html += "	<div id=':19a' class='ii gt'>                                                                                                                                                                                                           ";
                                    html += "		<div id=':199' class='a3s aXjCH '><u></u>                                                                                                                                                                                           ";
                                    html += "			<div bgcolor='#fdfdfd' style='text-align:center'>                                                                                                                                                                               ";
                                    html += "				<table border='0' width='900' height='100%' cellpadding='0' cellspacing='0' align='center'>                                                                                                                                 ";
                                    html += "					<tbody>                                                                                                                                                                                                                 ";
                                    html += "						<tr>                                                                                                                                                                                                                ";
                                    html += "							<td>                                                                                                                                                                                                            ";
                                    html += "								<div style='height:20px'>&nbsp;</div>                                                                                                                                                                       ";
                                    html += "							</td>                                                                                                                                                                                                           ";
                                    html += "						</tr>                                                                                                                                                                                                               ";
                                    html += "						<tr>                                                                                                                                                                                                                ";
                                    html += "							<td align='center' valign='top>                                                                                                                                                                                 ";
                                    html += "                    <table border=' width='900' cellpadding='0' cellspacing='0' bgcolor='#ffffff'></td>                                                                                                                                    ";
                                    html += "						</tr>                                                                                                                                                                                                               ";
                                    html += "					</tbody>                                                                                                                                                                                                                ";
                                    html += "					<tbody>                                                                                                                                                                                                                 ";
                                    html += "						<tr>                                                                                                                                                                                                                ";
                                    html += "							<td>                                                                                                                                                                                                            ";
                                    html += "								<img src='https://ci6.googleusercontent.com/proxy/saWNx6fZlUoNenc8b0X_5va2eQ09rKekOzJPSqViWi27fcNL7OWi-ESMx4KelZI4ax2y7jfWsaqdrcg=s0-d-e1-ft#https://keyiflibilet.com/mail/header.png' class='CToWUd'>      ";
                                    html += "							</td>                                                                                                                                                                                                           ";
                                    html += "							<td></td>                                                                                                                                                                                                       ";
                                    html += "						</tr>                                                                                                                                                                                                               ";
                                    html += "						<tr>                                                                                                                                                                                                                ";
                                    html += "							<td colspan='2' bgcolor='#ffffff' style='background-color:#ffffff;padding-left:30px;padding-right:30px;font-size:13px;line-height:20px;font-family:Helvetica,sans-serif;color:#333' align='left'>               ";
                                    html += "								<br>                                                                                                                                                                                                        ";
                                    html += "								<h3>Bilet Bilgileriniz</h3>                                                                                                                                                                                 ";
                                    html += "								<table cellpadding='5' cellspacing='5'>                                                                                                                                                                     ";
                                    html += "									<thead>                                                                                                                                                                                                 ";
                                    html += "										<tr>                                                                                                                                                                                                ";
                                    html += "											<th>İsim Soyisim</th>                                                                                                                                                                           ";
                                    html += "											<th>PNR</th>                                                                                                                                                                                    ";
                                    html += "											<th>Bilet Numarası</th>                                                                                                                                                                         ";
                                    html += "										</tr>                                                                                                                                                                                               ";
                                    html += "									</thead>                                                                                                                                                                                                ";
                                    html += "									<tbody> ";
                                    foreach (var item in orderrr.passengers)
                                    {
                                        html += "										<tr>      ";
                                        html += "											<td>" + item.name + " " + item.surname + @"</td>                                                                                                                                                                             ";
                                        html += "											<td>" + orderrr.BookingInfo.FirstOrDefault(w => w.Id == id).pnr + @"</td>                                                                                                                                                                                 ";
                                        html += "											<td>" + item.eticket + @"</td>                                                                                                                                                                          ";

                                        html += "										</tr>   ";
                                    }

                                    html += "									</tbody>                                                                                                                                                                                                ";
                                    html += "									<tbody>                                                                                                                                                                                                 ";
                                    html += "										<tr>                                                                                                                                                                                                ";
                                    html += "											<th>&nbsp;</th>                                                                                                                                                                                 ";
                                    html += "											<td>&nbsp;</td>                                                                                                                                                                                 ";
                                    html += "										</tr>                                                                                                                                                                                               ";
                                    html += "									</tbody>                                                                                                                                                                                                ";
                                    html += "								</table>                                                                                                                                                                                                    ";
                                    html += "								<br>                                                                                                                                                                                                        ";
                                    html += "								<h3>Bilet Bilgileriniz</h3>                                                                                                                                                                                 ";
                                    html += "								<table cellpadding='5' cellspacing='5'>                                                                                                                                                                     ";
                                    html += "									<tbody>                                                                                                                                                                                                 ";
                                    foreach (var item in orderrr.legs)
                                    {
                                        if (item.returnFlight == false)
                                        {
                                            html += "										<tr>";
                                            html += "											<td>                                                                                                                                                                                            ";
                                            html += "												<table cellpadding='5' cellspacing='5'>                                                                                                                                                     ";
                                            html += "													<thead>                                                                                                                                                                                 ";
                                            html += "														<tr>                                                                                                                                                                                ";
                                            html += "															<th colspan='8'>Gidiş Uçuş Bilgileri</th>                                                                                                                                       ";
                                            html += "														</tr>                                                                                                                                                                               ";
                                            html += "														<tr>                                                                                                                                                                                ";

                                            html += "															<th>Havayolu</th>                                                                                                                                                               ";
                                            html += "															<th>Uçuş No</th>                                                                                                                                                                ";

                                            html += "															<th>Kalkış Havalimanı</th>                                                                                                                                                      ";
                                            html += "															<th>Kalkış Tarih Saat</th>                                                                                                                                                      ";
                                            html += "															<th>Varış Havalimanı</th>                                                                                                                                                       ";
                                            html += "															<th>Varış Saati</th>                                                                                                                                                            ";
                                            html += "														</tr>                                                                                                                                                                               ";
                                            html += "													</thead>                                                                                                                                                                                ";
                                            html += "													<tbody>                                                                                                                                                                                 ";
                                            html += "														<tr>";
                                            html += "															<td></td>                                                                                                                                                                       ";
                                            html += "															<td>" + item.operatorName + @"</td>                                                                                                                                                             ";
                                            html += "															<td>" + item.flightNo + @"</td>                                                                                                                                                                 ";


                                            html += "															<td>" + item.arrivalCityName + " " + (item.arrivalAirport) + @"</td>";
                                            html += "															<td>" + Convert.ToDateTime(item.localArrivalDate).ToShortDateString() + " " + Convert.ToDateTime(item.localArrivalDate).ToShortTimeString() + @"</td>";
                                            html += "														</tr>";
                                            html += "													</tbody>                                                                                                                                                                                ";
                                            html += "												</table>                                                                                                                                                                                    ";
                                            html += "											</td>                                                                                                                                                                                           ";
                                            html += "										</tr>";
                                        }
                                        else
                                        {
                                            html += "										<tr>";
                                            html += "											<td>                                                                                                                                                                                            ";
                                            html += "												<table cellpadding='5' cellspacing='5'>                                                                                                                                                     ";
                                            html += "													<thead>                                                                                                                                                                                 ";
                                            html += "														<tr>                                                                                                                                                                                ";
                                            html += "															<th colspan='8'>Dönüş Uçuş Bilgileri</th>                                                                                                                                       ";
                                            html += "														</tr>                                                                                                                                                                               ";
                                            html += "														<tr>                                                                                                                                                                                ";
                                            html += "															                                                                                                                                                                  ";
                                            html += "															<th>Havayolu</th>                                                                                                                                                               ";
                                            html += "															<th>Uçuş No</th>                                                                                                                                                                ";
                                         
                                            html += "															<th>Kalkış Havalimanı</th>                                                                                                                                                      ";
                                            html += "															<th>Kalkış Tarih Saat</th>                                                                                                                                                      ";
                                            html += "															<th>Varış Havalimanı</th>                                                                                                                                                       ";
                                            html += "															<th>Varış Saati</th>                                                                                                                                                            ";
                                            html += "														</tr>                                                                                                                                                                               ";
                                            html += "													</thead>                                                                                                                                                                                ";
                                            html += "													<tbody>                                                                                                                                                                                 ";
                                            html += "	                                                  <tr > ";
                     
                                            html += "															<td>" + item.operatorName + @"</td>                                                                                                                                                             ";
                                            html += "															<td>" + item.flightNo + @"</td>                                                                                                                                                                 ";


                                            html += "															<td>" + item.arrivalCityName + " " + (item.arrivalAirport) + @"</td>";
                                            html += "															<td>" + Convert.ToDateTime(item.localArrivalDate).ToShortDateString() + " " + Convert.ToDateTime(item.localArrivalDate).ToShortTimeString() + @"</td>";
                                            html += "														</tr>";
                                            html += "													</tbody>                                                                                                                                                                                ";
                                            html += "												</table>                                                                                                                                                                                    ";
                                            html += "											</td>                                                                                                                                                                                           ";
                                            html += "										</tr>";
                                        }
                                    }
                                    html += "									</tbody>                                                                                                                                                                                                ";
                                    html += "								</table>                                                                                                                                                                                                    ";
                                    html += "							</td>                                                                                                                                                                                                           ";
                                    html += "						</tr>                                                                                                                                                                                                               ";
                                    html += "					</tbody>                                                                                                                                                                                                                ";
                                    html += "				</table>                                                                                                                                                                                                                    ";
                                    html += "				<table>                                                                                                                                                                                                                     ";
                                    html += "					<tbody>                                                                                                                                                                                                                 ";
                                    html += "						<tr>                                                                                                                                                                                                                ";
                                    html += "							<td>                                                                                                                                                                                                            ";
                                    html += "								<div style='height:40px'></div>                                                                                                                                                                             ";
                                    html += "							</td>                                                                                                                                                                                                           ";
                                    html += "						</tr>                                                                                                                                                                                                               ";
                                    html += "					</tbody>                                                                                                                                                                                                                ";
                                    html += "				</table>                                                                                                                                                                                                                    ";
                                    html += "				<div class='yj6qo'></div>                                                                                                                                                                                                   ";
                                    html += "				<div class='adL'></div>                                                                                                                                                                                                     ";
                                    html += "			</div>                                                                                                                                                                                                                          ";
                                    html += "			<div class='adL'></div>                                                                                                                                                                                                         ";
                                    html += "		</div>                                                                                                                                                                                                                              ";
                                    html += "	</div>                                                                                                                                                                                                                                  ";
                                    html += "	<div id=':1b7' class='ii gt' style='display:none'>                                                                                                                                                                                      ";
                                    html += "		<div id=':1b8' class='a3s aXjCH undefined'></div>                                                                                                                                                                                   ";
                                    html += "	</div>                                                                                                                                                                                                                                  ";
                                    //Bileti Maile gönderme
                                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                                                              new System.Net.Mail.MailAddress("oget.metin@gmail.com", "Keyifli Bilet"),
                                                              new System.Net.Mail.MailAddress("oget.metin@gmail.com"));


                                    m.Subject = "Keyifli Bilet";
                                    m.Body = "pnr nolu biletiniz : " + html;
                                    m.IsBodyHtml = true;
                                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                                    smtp.Credentials = new System.Net.NetworkCredential("oget.metin@gmail.com", "Rizgar1341?");
                                    smtp.EnableSsl = true;
                                    smtp.Send(m);
                                    
                                   
                                    //SMS Kontrolü
                                    var smsIstegi = new SmsIstegi();
                                    smsIstegi.username = "908505322075";
                                    smsIstegi.password = "Arslan123+";
                                    smsIstegi.source_addr = "08505322075";
                                    smsIstegi.messages = new Mesaj[] { new Mesaj("Pnr : " + order.result.bookingInfo.pnr + " Nolu Biletiniz başarılı bir şekilde oluşturuldu", "90" + contact.mobilePhoneNumber) };
                                    IstegiGonder(smsIstegi);
                                    //Index View Model Doldurma
                                    var Id = entities.OrderResult.Where(w => w.orderId == getorder.orderId).FirstOrDefault().Id;
                                    vm.booking = entities.BookingInfo.Where(w => w.Id == Id).ToList();
                                    vm.leg = entities.legs.Where(w => w.Id == Id).ToList();
                                    vm.orderResult = entities.OrderResult.Where(w => w.Id == Id).ToList();
                                    vm.passenger = entities.passengers.Where(w => w.Id == Id).ToList();
                                }
                                else
                                {
                                    string dest = "905435211321,905064769460,905374105222";
                                    var smsIstegi = new SmsIstegi();
                                    smsIstegi.username = "908505322075";
                                    smsIstegi.password = "Arslan123+";
                                    smsIstegi.source_addr = "08505322075";
                                    smsIstegi.messages = new Mesaj[] { new Mesaj("Mesaj : " + "RES NULL GELİYOR İATİ İLE İLETİŞİMİNE GEÇİN Misafir Telefon No: "+contact.mobilePhoneNumber, dest) };
                                    IstegiGonder(smsIstegi);
                                    return RedirectToAction("BankInfo", new { message = "BİLETLEME YAPILAMADI ÇAĞRI MERKEZİMİZ SİZİNLE İLETİŞİME GEÇECEKTİR. " });
                                }
                            }
                            catch (TicketException ex)
                            {

                                //Bileti Maile gönderme
                                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                                                         new System.Net.Mail.MailAddress("oget.metin@gmail.com", "Keyifli Bilet Iati MakeTicket Servisi Hatası"),
                                                         new System.Net.Mail.MailAddress("oget.metin@gmail.com"));


                                m.Subject = "Keyifli Bilet";
                                m.Body = string.Format(ex.InnerException + "  :  " + ex.Message + " :  hatası", Request.Url.Scheme);
                                m.IsBodyHtml = true;
                                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                                smtp.Credentials = new System.Net.NetworkCredential("oget.metin@gmail.com", "Rizgar1341?");
                                smtp.EnableSsl = true;
                                smtp.Send(m);
                                string dest = "905435211321,905064769460,905374105222";
                                //SMS Kontrolü
                                var smsIstegi = new SmsIstegi();
                                smsIstegi.username = "908505322075";
                                smsIstegi.password = "Arslan123+";
                                smsIstegi.source_addr = "08505322075";
                                smsIstegi.messages = new Mesaj[] { new Mesaj("Mesaj : " + "İATİ İLE İLETİŞİMİNE GEÇİN Misafir Telefon No: "+contact.mobilePhoneNumber, dest) };
                                IstegiGonder(smsIstegi);
                                return RedirectToAction("BankInfo", new { message = "BİLETLEME YAPILAMADI ÇAĞRI MERKEZİMİZ SİZİNLE İLETİŞİME GEÇECEKTİR. " });
                            }


                        }
                        catch (Exception ex)
                        {

                            //Bileti Maile gönderme
                            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                                                     new System.Net.Mail.MailAddress("oget.metin@gmail.com", "Keyifli Bilet Iati MakeTicket Servisi Hatası"),
                                                     new System.Net.Mail.MailAddress("oget.metin@gmail.com"));


                            m.Subject = "Keyifli Bilet";
                            m.Body = string.Format(ex.InnerException + "  :  " + ex.Message + " :  hatası", Request.Url.Scheme);
                            m.IsBodyHtml = true;
                            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                            smtp.Credentials = new System.Net.NetworkCredential("oget.metin@gmail.com", "Rizgar1341?");
                            smtp.EnableSsl = true;
                            smtp.Send(m);
                            string dest = "905435211321,905064769460,905374105222";
                            //SMS Kontrolü
                            var smsIstegi = new SmsIstegi();
                            smsIstegi.username = "908505322075";
                            smsIstegi.password = "Arslan123+";
                            smsIstegi.source_addr = "08505322075";
                            smsIstegi.messages = new Mesaj[] { new Mesaj("Mesaj : " + "İATİ İLE İLETİŞİMİNE GEÇİN Misafir Telefon No: " + contact.mobilePhoneNumber, dest) };
                            IstegiGonder(smsIstegi);
                            return RedirectToAction("BankInfo", new { message = "BİLETLEME YAPILAMADI ÇAĞRI MERKEZİMİZ SİZİNLE İLETİŞİME GEÇECEKTİR. " });
                        }
                    }
                    else
                    {

                        //Banka (Limit Yetersiz vm) Ekranı Yapılacak
                       return RedirectToAction("BankInfo", new { message = xmlErrMsg });
                    }
                    resp.Close();



                }
                catch (Exception ex)
                {
                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                                                        new System.Net.Mail.MailAddress("oget.metin@gmail.com", "Keyifli Bilet Iati MakeTicket Servisi Hatası"),
                                                        new System.Net.Mail.MailAddress("oget.metin@gmail.com"));


                    m.Subject = "Keyifli Bilet Banka Onayı Alınamadı";
                    m.Body = string.Format(ex.InnerException + "  :  " + ex.Message + " :  hatası", Request.Url.Scheme);
                    m.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential("oget.metin@gmail.com", "Rizgar1341?");
                    smtp.EnableSsl = true;
                    smtp.Send(m);

                    //SMS Kontrolü
                    string dest = "905435211321,905064769460,905374105222";
                    var smsIstegi = new SmsIstegi();
                    smsIstegi.username = "908505322075";
                    smsIstegi.password = "Arslan123+";
                    smsIstegi.source_addr = "08505322075";
                    smsIstegi.messages = new Mesaj[] { new Mesaj("Mesaj : " + contact.mobilePhoneNumber + "" + ex.Message + " sanal pos hatası oluştu kişi ile iletişime geçin", dest) };
                    IstegiGonder(smsIstegi);
                    return RedirectToAction("BankInfo", new { message = "Teknik Bir Sorun Oluştu. Çağrı Merkezimiz sizinle iletişime geçecektir." });

                }
                finally
                {
                    if (resp != null)
                        resp.Close();
                }

            }
            else
            {
                string dest = "905435211321,905064769460,905374105222";
                var smsIstegi = new SmsIstegi();
                smsIstegi.username = "908505322075";
                smsIstegi.password = "Arslan123+";
                smsIstegi.source_addr = "08505322075";
                smsIstegi.messages = new Mesaj[] { new Mesaj("Mesaj : " + contact.mobilePhoneNumber +" sanal pos hatası oluştu Kart Bilgileri Yanlış girildi. kişi ile iletişime geçin", dest) };
                IstegiGonder(smsIstegi);
                return RedirectToAction("BankInfo", new { message = "Teknik Bir Sorun Oluştu. Çağrı Merkezimiz sizinle iletişime geçecektir." });
            }

            return View(vm);
        }
        public ActionResult BankInfo(string message)
        {
            ViewBag.Message = message;

            return View();
        }

        private void IstegiGonder(SmsIstegi istek)
        {
            string payload = JsonConvert.SerializeObject(istek);

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Headers["Content-Type"] = "application/json";

            try
            {
                string campaign_id = wc.UploadString("http://sms.verimor.com.tr/v2/send.json", payload);

            }
            catch (WebException ex) // 400 hatalarında response body'de hatanın ne olduğunu yakalıyoruz
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) // 400 hataları
                {
                    var responseBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    //Hatayı Mail attırma
                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                                           new System.Net.Mail.MailAddress("oget.metin@gmail.com", "Keyifli Bilet SMS Hata Kaydı Maili"),
                                           new System.Net.Mail.MailAddress("oget.metin@gmail.com"));


                    m.Subject = "Keyifli Bilet Hata Kaydı Maili";
                    m.Body = string.Format(responseBody, Request.Url.Scheme);
                    m.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new System.Net.NetworkCredential("oget.metin@gmail.com", "Rizgar1341?");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                }
               
            }
        }

        public PartialViewResult HeaderResult()
        {
            return PartialView();
        }
        public PartialViewResult FilterOneWay()
        {

            return PartialView();
        }
        public PartialViewResult Departure()
        {
            return PartialView();
        }
        public PartialViewResult Arrival()
        {
            return PartialView();
        }
        public PartialViewResult FareDetails(List<Iati.Response.Fare> model)
        {
            ViewBag.ViewSearch = ViewSearch;

            return PartialView(model);
        }
        public PartialViewResult Person()
        {
            return PartialView();
        }
        public PartialViewResult CardInfo()
        {
            return PartialView();
        }

        public class Mesaj
        {
            public string msg { get; set; }
            public string dest { get; set; }

            public Mesaj() { }

            public Mesaj(string msg, string dest)
            {
                this.msg = msg;
                this.dest = dest;
            }
        }
        public class SmsIstegi
        {
            public string username { get; set; }
            public string password { get; set; }
            public string source_addr { get; set; }
            public Mesaj[] messages { get; set; }
        }
        public class Hata
        {
            public string errorCode { get; set; }
            public string errorTitle { get; set; }
            public string description { get; set; }
            public string details { get; set; }
            public string reference { get; set; }
        }
    }
}