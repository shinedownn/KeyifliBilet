using KeyifliBilet.Areas.Yonetim.Models;
using KeyifliBilet.Iati.Request;
using KeyifliBilet.Iati.Response;
using KeyifliBilet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace KeyifliBilet.Controllers
{

    public class HomeController : Controller
    {
        static List<string> result;
        IndexViewModel vm = new IndexViewModel();
        KeyifliBiletEntities entities = new KeyifliBiletEntities();


        public PartialViewResult HeaderIndex()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult KayitOl(FormCollection data)
        {
            var mail = data["txtReqEmail"];
            var gsm = data["txtReqMobile"];
            int uyeCon = entities.Uyeler.Where(w => w.Mail == mail && w.GSM == gsm).Count();
            if (uyeCon == 0)
            {
                Uyeler uye = new Uyeler();
                uye.AdSoyad = data["Name"] + " " + data["Surname"];
                uye.Durum = false;
                uye.GSM = data["txtReqMobile"];
                uye.KampanyaDurum = true;
                uye.KayitTarihi = DateTime.Now;
                uye.Mail = data["txtReqEmail"];
                uye.Sifre = data["Password"];
                entities.Uyeler.Add(uye);
                entities.SaveChanges();

                var user = entities.Uyeler.Where(w => w.Mail == mail).First();
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                                            new System.Net.Mail.MailAddress("oget.metin@gmail.com", "Keyifli Bilet Kayıt Maili"),
                                            new System.Net.Mail.MailAddress(mail));


                m.Subject = "Email onaylama";
                m.Body = string.Format("Sayın {0}<BR/>Kayıt olduğunuz için teşekkürler, lütfen kaydınızı doğrulamak için adrese tıklayınız: <a href=\"{1}\" title=\"Kullanıcı E-mail Doğrulama\">{1}</a>", uye.Mail, Url.Action("Index", "Home", new { Token = user.UyeId, Email = user.Mail }, Request.Url.Scheme));
                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new System.Net.NetworkCredential("oget.metin@gmail.com", "Rizgar1341?");
                smtp.EnableSsl = true;
                smtp.Send(m);
            }
            else
            {
                //Üye Zaten Kayıtlı
            }


            return RedirectToAction("Index", "Home");

        }

        public ActionResult Cikis()
        {
            Session["UyeId"] = null;

            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public class Order
        {
            public string Tk { get; set; }
            public string Ad { get; set; }
            public string Soyad { get; set; }
            public string Kalkis { get; set; }
            public string Varis { get; set; }
            public string Tarih { get; set; }
            public string Saat { get; set; }
            public string Sinif { get; set; }
            public string Ucret { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult> PnrSorgulama(FormCollection data)
        {
            var pnr = data["pnr"];
            var soyad = data["soyad"];

            var pnrId = entities.BookingInfo.Where(w => w.pnr == pnr.ToUpper()).FirstOrDefault();
            var Id = entities.BookingInfo.Where(w => w.pnr == pnr.ToUpper()).FirstOrDefault().Id;
            var trrr = entities.OrderResult.FirstOrDefault(w => w.Id == Id).orderId;
            var yolcu = entities.passengers.Where(w => w.surname.ToUpper() == soyad.ToUpper() && w.Id == Id).FirstOrDefault();

            GetOrderResponse order = null;
            if (yolcu != null)
            {
                order = await Iati.Search.GetOrder(new GetOrderRequest { orderId = Convert.ToInt32(trrr) });
            }
            else
            {
                order = null;
                //pnr ve ya soyadını kontrol ediniz.
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult Giris(FormCollection data)
        {
            var mail = data["txtLogEmail"];
            var sifre = data["txtLogPassword"];
            var login = entities.Uyeler.Where(m => m.Mail == mail).FirstOrDefault();
            if (login != null)
            {
                if (login.Mail == mail && login.Sifre == sifre && login.Durum == true)
                {
                    Session["UyeId"] = login.UyeId;
                    Session["AdSoyad"] = login.AdSoyad;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Uyari = "Kullanıcı Adı veya Şifreyi Kontrol Ediniz / Yeni Kayıt Olduysanız ONAY Verilmesini Beklemelisiniz!";
                    return RedirectToAction("Index", "Home");

                }
            }
            else
            {
                ViewBag.Uyari = "Kullanıcı Adı veya Şifreyi Kontrol Ediniz / Yeni Kayıt Olduysanız ONAY Verilmesini Beklemelisiniz!";
                return RedirectToAction("Index", "Home");

            }

        }

        [HttpPost]
        public ActionResult BizeUlasin(FormCollection data)
        {
            IletisimFormu ulas = new IletisimFormu();
            ulas.AdSoyad = data["NameSurname"];
            ulas.Mail = data["Email"];
            ulas.Telefon = data["Mobile"];
            ulas.Mesaj = data["Message"];
            ulas.Durum = false;
            ulas.OkunmaTarihi = null;
            ulas.Tarih = DateTime.Now;
            entities.IletisimFormu.Add(ulas);
            entities.SaveChanges();
            return View("Index");
        }

        public ActionResult Biletlerim()
        {
            int sesion = Convert.ToInt32(Session["UyeId"]);

            vm.uyeler = entities.Uyeler.Where(w => w.UyeId == sesion);
            var data = entities.OrderResult.FirstOrDefault(w => w.UyeId == sesion).Id;
            vm.leg = entities.legs.Where(w => w.Id == data).ToList();

            return View(vm);
        }

        public ActionResult Profil()
        {
            int sesion = Convert.ToInt32(Session["UyeId"]);
            vm.uyeler = entities.Uyeler.Where(w => w.UyeId == sesion).ToList();
            return View(vm);
        }
        [HttpPost]
        public ActionResult Profil(FormCollection data)
        {
            var sifre = data["mevcut"];
            int UyeId = Convert.ToInt32(Session["UyeId"]);
            var uyeId = entities.Uyeler.Where(w => w.UyeId == UyeId).FirstOrDefault();
            if (sifre!=null)
            {
                string t = entities.Uyeler.Where(w => w.UyeId == UyeId).FirstOrDefault().Sifre;
                if (t==sifre)
                {
                    uyeId.Sifre = data["yeni"];
                    entities.SaveChanges();
                }
            }
            else
            {
                
                uyeId.AdSoyad = data["txtUpName"];
                uyeId.GSM = data["txtUpMobile"];
                uyeId.Mail = data["txtUpEmail"];
                entities.SaveChanges();
            }
          
            return View(vm);
        }


        [HttpPost]
        public ActionResult SifremiUnuttum(FormCollection form)
        {
            string mail = form["Email"].Trim();
            var data = entities.Uyeler.Where(w => w.Mail == mail).First();
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                  new System.Net.Mail.MailAddress("oget.metin@gmail.com", "Şifreniz ve Kullanıcı Adınız"),
                  new System.Net.Mail.MailAddress(data.Mail));


            m.Subject = "Email onaylama";
            m.Body = string.Format(" Kullanıcı Adınız : {0}<BR/> Şifreniz : {1}<BR/> ", data.Mail, data.Sifre);
            m.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential("oget.metin@gmail.com", "Rizgar1341?");
            smtp.EnableSsl = true;
            smtp.Send(m);
            return RedirectToAction("Index", "Home");
        }


        public PartialViewResult Slider()
        {
            return PartialView();
        }
        public PartialViewResult PartialSearch()
        {
            var model = new ViewSearch();
            return PartialView(model);
        }
        public PartialViewResult PartialSearchWithModel(ViewSearch model)
        {

            return PartialView(model);
        }
        [HttpPost]
        public async Task<JsonResult> Calendar(ViewSearch model)
        {
            var cal = new GetCalendarRequest();
            if (Convert.ToDateTime(model.DepartureDate).DayOfYear - DateTime.Now.DayOfYear >= 3)
            {
                cal.departureDateTimeBegin = Convert.ToDateTime(model.DepartureDate).AddDays(-4).ToString("yyyy-MM-dd");
                cal.departureDateTimeEnd = Convert.ToDateTime(model.DepartureDate).AddDays(3).ToString("yyyy-MM-dd");
                cal.returnDateTimeBegin = model.ReturnDate != null ? Convert.ToDateTime(model.ReturnDate).AddDays(-4).ToString("yyyy-MM-dd") : "";
                cal.returnDateTimeEnd = model.ReturnDate != null ? Convert.ToDateTime(model.ReturnDate).AddDays(3).ToString("yyyy-MM-dd") : "";
                cal.currency = "TL";
                cal.routes = new List<GetCalendar>() {
                    new GetCalendar(){
                        fromAirportCode =model.DEP_Airport_Code,
                        fromAirportIsCity =false,
                        toAirportCode =model.ARR_Airport_Code,
                        toAirportIsCity =false
                    }
                };
            }
            else
            {
                cal.departureDateTimeBegin = Convert.ToDateTime(model.DepartureDate).ToString("yyyy-MM-dd");
                cal.departureDateTimeEnd = Convert.ToDateTime(model.DepartureDate).AddDays(7).ToString("yyyy-MM-dd");
                cal.returnDateTimeBegin = model.ReturnDate != null ? Convert.ToDateTime(model.ReturnDate).ToString("yyyy-MM-dd") : "";
                cal.returnDateTimeEnd = model.ReturnDate != null ? Convert.ToDateTime(model.ReturnDate).AddDays(7).ToString("yyyy-MM-dd") : "";
                cal.currency = "TL";
                cal.routes = new List<GetCalendar>() {
                    new GetCalendar(){
                        fromAirportCode =model.DEP_Airport_Code,
                        fromAirportIsCity =false,
                        toAirportCode =model.ARR_Airport_Code,
                        toAirportIsCity =false
                    }
                };
            }

            var calendar = await Iati.Search.GetCalendar(cal);
            var r = Json(calendar, JsonRequestBehavior.AllowGet);
            return r;
        }

        public PartialViewResult PartialPNR()
        {
            return PartialView();
        }
        public async Task<JsonResult> Destinations()
        {
            var kutu = entities.Kutu;
            List<ViewSearch> viewlist = new List<ViewSearch>();
            List<KeyifliBilet.Iati.Response.FlightResponse> flights = new List<FlightResponse>();
            foreach (var item in kutu)
            {
                ViewSearch view = new ViewSearch();
                view.Adult = 1;
                view.ARR_Airport_CityName = item.ARR_Airport_CityName;
                view.ARR_Airport_Code = item.ARR_Airport_Code;
                view.ARR_Airport_IsCity = false;
                view.ARR_Airport_Name = item.ARR_Airport_Name;
                view.CabinClass = null;
                view.Child = 0;
                view.DepartureDate = DateTime.Now.AddMonths(1).ToShortDateString();
                view.DEP_Airport_CityName = item.DEP_Airport_CityName;
                view.DEP_Airport_Code = item.DEP_Airport_Code;
                view.DEP_Airport_IsCity = false;
                view.DEP_Airport_Name = item.DEP_Airport_Name;
                view.Infant = 0;
                view.IsOneWay = true;
                view.NoStop = false;
                view.ReturnDate = "";
                view.Total = 1;
                viewlist.Add(view);
            }
            foreach (var model in viewlist)
            {
                var f = await Iati.Search.GetFlights(new FlightRequest()
                {
                    adult = model.Adult.ToString(),
                    allinFromCity = model.DEP_Airport_IsCity,
                    allinToCity = model.ARR_Airport_IsCity,
                    child = model.Child.ToString(),
                    cip = false,
                    classType = model.CabinClass == "ALL" ? null : (Iati.Enums.ClassType?)Enum.Parse(typeof(Iati.Enums.ClassType), model.CabinClass),
                    currency = Iati.Enums.Currency.TL,
                    fromAirport = model.DEP_Airport_Code,
                    fromDate = Convert.ToDateTime(model.DepartureDate).AddMonths(1).ToString("yyyy-MM-dd"),
                    getBaggageInfo = false,
                    infant = model.Infant.ToString(),
                    returnDate = "",
                    toAirport = model.ARR_Airport_Code,
                    usePersonFares = true
                });
                flights.Add(f);
            }
            return Json(flights);
        }

        public async Task<PartialViewResult> PartialDestenation()
        {
            vm.kutuKategori = entities.KutuKategori;
            var kutu = entities.Kutu;
            List<ViewSearch> viewlist = new List<ViewSearch>();
            List<KeyifliBilet.Iati.Response.FlightResponse> flights = new List<FlightResponse>();
            var tasks = new List<Task>();
            foreach (var item in kutu)
            {
                ViewSearch view = new ViewSearch();
                view.Adult = 1;
                view.ARR_Airport_CityName = item.ARR_Airport_CityName;
                view.ARR_Airport_Code = item.ARR_Airport_Code;
                view.ARR_Airport_IsCity = false;
                view.ARR_Airport_Name = item.ARR_Airport_Name;
                view.CabinClass = null;
                view.Child = 0;
                view.DepartureDate = DateTime.Now.AddMonths(1).ToShortDateString();
                view.DEP_Airport_CityName = item.DEP_Airport_CityName;
                view.DEP_Airport_Code = item.DEP_Airport_Code;
                view.DEP_Airport_IsCity = false;
                view.DEP_Airport_Name = item.DEP_Airport_Name;
                view.Infant = 0;
                view.IsOneWay = true;
                view.NoStop = false;
                view.ReturnDate = "";
                view.Total = 1;
                viewlist.Add(view);
            }
            //foreach (var model in viewlist)
            //{
            //    tasks.Add(Task.Run(() => Iati.Search.GetFlights(new FlightRequest() {
            //        adult = model.Adult.ToString(),
            //        allinFromCity = model.DEP_Airport_IsCity,
            //        allinToCity = model.ARR_Airport_IsCity,
            //        child = model.Child.ToString(),
            //        cip = false,
            //        classType = model.CabinClass == "ALL" ? null : (Iati.Enums.ClassType?)Enum.Parse(typeof(Iati.Enums.ClassType), model.CabinClass),
            //        currency = Iati.Enums.Currency.TL,
            //        fromAirport = model.DEP_Airport_Code,
            //        fromDate = Convert.ToDateTime(model.DepartureDate).AddMonths(1).ToString("yyyy-MM-dd"),
            //        getBaggageInfo = false,
            //        infant = model.Infant.ToString(),
            //        //returnDate = Convert.ToDateTime(model.ReturnDate).ToString("yyyy-MM-dd"),
            //        toAirport = model.ARR_Airport_Code,
            //        usePersonFares = true
            //    })));
            //}
            //await Task.WhenAll(tasks);
            //ViewBag.Kutular = flights;
            ViewBag.viewlist = viewlist;
            vm.kutu = entities.Kutu;
            return PartialView();
        }

        public PartialViewResult Blog()
        {
            vm.sayfalar = entities.Sayfalar.Where(w => w.Ad == "BLOG");
            return PartialView(vm);
        }
        public PartialViewResult Features()
        {
            return PartialView();
        }
        public PartialViewResult SignUp()
        {
            return PartialView();
        }
        public PartialViewResult PartialSignUp()
        {
            return PartialView();
        }
        public PartialViewResult FlightImages()
        {
            return PartialView();
        }
        public PartialViewResult Footer()
        {
            return PartialView();
        }

        public PartialViewResult HeaderResult()
        {
            return PartialView();
        }
 
        public ActionResult Index()
        {
         
            Uri myuri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            string pathQuery = myuri.PathAndQuery;
            string hostName = myuri.ToString().Replace(pathQuery, "");
            string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpPost]
        public JsonResult About(string fromAirport, string toAirport)
        {
            result = new List<string>() { fromAirport, toAirport };
            return Json(new { Result = true, url = "/" + fromAirport + "-" + toAirport }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

      
    }
}