using KeyifliBilet.Areas.Yonetim.Models;
using KeyifliBilet.Iati.Enums;
using KeyifliBilet.Iati.Request;
using KeyifliBilet.Iati.Response;
using KeyifliBilet.Models;
using KeyifliBiletPanel.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using static KeyifliBilet.Areas.Yonetim.Controllers.PanelController.Enum;

namespace KeyifliBilet.Areas.Yonetim.Controllers
{

    public class PanelController : Controller
    {
        IndexViewModel vm = new IndexViewModel();
        KeyifliBiletEntities entities = new KeyifliBiletEntities();
        public static int id = 0;
        // GET: Panel
        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Anasayfa")]
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var res = await Iati.Search.GetBalanceSummary();
            var uye = entities.Uyeler.Count();
            var bilet = entities.BookingInfo.Count();
            var kutu = entities.Kutu.Count();
            ViewBag.balance = res.result.balance.ToString() + " " + res.result.currency;
            ViewBag.uye = uye;
            ViewBag.bilet = bilet;
            ViewBag.kutu = kutu;
            return View();
        }

        public PartialViewResult UserName()
        {
            if (id == 0)
            {
                vm.uyeler = entities.Uyeler.Where(w => w.UyeId == id);
            }

            return PartialView(vm);
        }
        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Profil")]
        public ActionResult Profil()
        {
            int rolId = 0;
            var data = entities.Rol.Where(w => w.KullaniciId == id).FirstOrDefault();
            rolId = data.RolId;
            vm.kullanicilar = entities.Kullanicilar.Where(w => w.KullaniciId == id);
            vm.rol = entities.Rol.Where(w => w.KullaniciId == id);
            vm.yetki = entities.Yetki.Where(w => w.RolId == rolId).ToList();
            return View(vm);
        }

        #region ÜYELER
        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Uyeler")]
        public ActionResult Uyeler()
        {
            vm.uyeler = entities.Uyeler.ToList();
            return View(vm);
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "UyeDurum")]
        public ActionResult UyeAktifPasif(int? id)
        {
            var uye = entities.Uyeler.Where(w => w.UyeId == id).FirstOrDefault();
            if (uye.Durum == true)
            {
                uye.Durum = false;
            }
            else
            {
                uye.Durum = true;

            }
            entities.SaveChanges();
            return RedirectToAction("Uyeler");
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "KampanyaDurum")]
        public ActionResult KampanyaDurum(int? id)
        {
            var uye = entities.Uyeler.Where(w => w.UyeId == id).FirstOrDefault();
            if (uye.KampanyaDurum == true)
            {
                uye.KampanyaDurum = false;

            }
            else
            {
                uye.KampanyaDurum = true;

            }
            entities.SaveChanges();
            return RedirectToAction("Uyeler");
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "UyeGuncelleme")]
        public PartialViewResult UyeGuncelle(int UyeId)
        {

            vm.uyeler = entities.Uyeler.Where(w => w.UyeId == UyeId);
            return PartialView(vm);

        }

        [MyFilterAdmin]
        [HttpPost]
        [KullaniciYetkiler(Roles = "UyeGuncelleme")]
        public ActionResult UyeGuncelle(FormCollection data, int? uyeId)
        {
            var uye = entities.Uyeler.Where(w => w.UyeId == uyeId).FirstOrDefault();
            var durum = entities.Uyeler.Where(w => w.UyeId == uyeId).FirstOrDefault().Durum;
            var kampanya = entities.Uyeler.Where(w => w.UyeId == uyeId).FirstOrDefault().KampanyaDurum;
            var sifre = entities.Uyeler.Where(w => w.UyeId == uyeId).FirstOrDefault().Sifre;
            var kayit = entities.Uyeler.Where(w => w.UyeId == uyeId).FirstOrDefault().KayitTarihi;

            uye.AdSoyad = data["adsoyad"];
            uye.Mail = data["mail"];
            uye.GSM = data["telefon"];
            uye.KayitTarihi = kayit;
            uye.Durum = durum;
            uye.KampanyaDurum = kampanya;
            uye.Sifre = sifre;
            entities.SaveChanges();
            vm.uyeler = entities.Uyeler.ToList();

            return View("Uyeler", vm);
        }

        #endregion
        public abstract class BaseController : Controller
        {

            public void Alert(string message, NotificationType notificationType)
            {
                var msg = "swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
                TempData["notification"] = msg;
            }
        }
        public class Enum
        {
            public enum NotificationType
            {
                error,
                success,
                warning,
                info
            }

        }
        #region EKSTRA SERVİS BEDELİ
        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "EkstraServisBedeli")]
        public ActionResult EkstraServisBedeli()
        {
            vm.ekstraServis = entities.EkstraServis.ToList();
            return View(vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "EkstraServisBedeliEkle")]
        public PartialViewResult ServisEkle()
        {
            return PartialView();
        }
        [HttpPost]
        [KullaniciYetkiler(Roles = "EkstraServisBedeliEkle")]
        public PartialViewResult ServisEkle(FormCollection data)
        {
            EkstraServis servis = new EkstraServis();
            string kalkisYeri = data["kalkisYeri"];
            string varisYeri = data["varisYeri"];
            string[] kalkis = kalkisYeri.Split('-');
            string[] varis = varisYeri.Split('-');



            if (data["myCheck"] != null)
            {
                // 1.Ekstra Servis Bedeli eklenecek
                servis.BaslangicTarihi = Convert.ToDateTime(data["baslangicTarihi"]);
                servis.BitisTarihi = Convert.ToDateTime(data["bitisTarihi"]);
                servis.DonusEklensinMi = true;
                servis.KalkisKod = kalkis[0];
                servis.KalkisHavalimani = kalkis[1];
                servis.KalkisSehir = kalkis[2];
                servis.KalkisUlke = kalkis[3];

                servis.KoltukLimiti = Convert.ToInt32(data["koltukLimiti"]);
                servis.LimiteBagliServisBedeli = Convert.ToDecimal(data["limiteBagliServis"]);
                servis.ServisBedeli = Convert.ToDecimal(data["ekstraServisBedeli"]);
                servis.Durum = true;
                if (data["tip"] == "İÇ HAT")
                {
                    servis.Tip = true;
                }
                else
                {
                    servis.Tip = false;
                }
                servis.VarisKod = varis[0];
                servis.VarisHavalimani = varis[1];
                servis.VarisSehir = varis[2];
                servis.VarisUlke = varis[3];
                entities.EkstraServis.Add(servis);
                entities.SaveChanges();
                // 2.Ekstra Servis Bedeli eklenecek
                servis.BaslangicTarihi = Convert.ToDateTime(data["baslangicTarihi"]);
                servis.BitisTarihi = Convert.ToDateTime(data["bitisTarihi"]);
                servis.DonusEklensinMi = true;
                servis.KalkisKod = varis[0];
                servis.KalkisHavalimani = varis[1];
                servis.KalkisSehir = varis[2];
                servis.KalkisUlke = varis[3];
                servis.KoltukLimiti = Convert.ToInt32(data["dkoltukLimiti"]);
                servis.LimiteBagliServisBedeli = Convert.ToDecimal(data["dlimiteBagliServis"]);
                servis.ServisBedeli = Convert.ToDecimal(data["dekstraServisBedeli"]);
                servis.Durum = true;
                if (data["tip"] == "İÇ HAT")
                {
                    servis.Tip = true;
                }
                else
                {
                    servis.Tip = false;
                }
                servis.VarisKod = kalkis[0];
                servis.VarisHavalimani = kalkis[1];
                servis.VarisSehir = kalkis[2];
                servis.VarisUlke = kalkis[3];
                entities.EkstraServis.Add(servis);
                entities.SaveChanges();
            }
            else
            {
                // Tek yön Ekstra Servis Bedeli ekleme
                servis.BaslangicTarihi = Convert.ToDateTime(data["baslangicTarihi"]);
                servis.BitisTarihi = Convert.ToDateTime(data["bitisTarihi"]);
                servis.DonusEklensinMi = false;
                servis.KalkisKod = kalkis[0];
                servis.KalkisHavalimani = kalkis[1];
                servis.KalkisSehir = kalkis[2];
                servis.KalkisUlke = kalkis[3];
                servis.KoltukLimiti = Convert.ToInt32(data["koltukLimiti"]);
                servis.LimiteBagliServisBedeli = Convert.ToDecimal(data["limiteBagliServis"]);
                servis.ServisBedeli = Convert.ToDecimal(data["ekstraServisBedeli"]);
                servis.Durum = true;
                if (data["tip"] == "İÇ HAT")
                {
                    servis.Tip = true;
                }
                else
                {
                    servis.Tip = false;
                }
                servis.VarisKod = varis[0];
                servis.VarisHavalimani = varis[1];
                servis.VarisSehir = varis[2];
                servis.VarisUlke = varis[3];
                entities.EkstraServis.Add(servis);
                entities.SaveChanges();
                vm.ekstraServis = entities.EkstraServis.ToList();
            }
            return PartialView(vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "EkstraServisBedeliGuncelleme")]
        public PartialViewResult ServisBedeliGuncelleme(int Id)
        {
            vm.ekstraServis = entities.EkstraServis.Where(w => w.Id == Id);
            return PartialView(vm);
        }
        [KullaniciYetkiler(Roles = "ServisDurum")]
        public ActionResult ServisAktifPasif(int? id)
        {
            var servis = entities.EkstraServis.Where(w => w.Id == id).FirstOrDefault();

            if (servis.Durum == true)
            {
                servis.Durum = false;
            }
            else
            {
                servis.Durum = true;
            }
            entities.SaveChanges();
            vm.ekstraServis = entities.EkstraServis.ToList();

            return RedirectToAction("EkstraServisBedeli", vm);
        }
        [HttpPost]
        [KullaniciYetkiler(Roles = "EkstraServisBedeliGuncelleme")]
        public PartialViewResult ServisBedeliGuncelleme(FormCollection data, int Id)
        {
            var servis1 = entities.EkstraServis.Where(w => w.Id == Id).FirstOrDefault();
            string kalkisYeri = data["kalkisYeri"];
            string varisYeri = data["varisYeri"];
            string[] kalkis = kalkisYeri.Split('-');
            string[] varis = varisYeri.Split('-');
            if (data["myCheck"] != null)
            {
                // 1.Ekstra Servis Bedeli güncellenecek
                servis1.BaslangicTarihi = Convert.ToDateTime(data["baslangicTarihi"]);
                servis1.BitisTarihi = Convert.ToDateTime(data["bitisTarihi"]);
                servis1.DonusEklensinMi = true;
                servis1.KalkisKod = kalkis[0];
                servis1.KalkisHavalimani = kalkis[1];
                servis1.KalkisSehir = kalkis[2];
                servis1.KalkisUlke = kalkis[3];
                servis1.KoltukLimiti = Convert.ToInt32(data["koltukLimiti"]);
                servis1.LimiteBagliServisBedeli = Convert.ToDecimal(data["limiteBagliServis"]);
                servis1.ServisBedeli = Convert.ToDecimal(data["ekstraServisBedeli"]);
                if (data["tip"] == "İÇ HAT")
                {
                    servis1.Tip = true;
                }
                else
                {
                    servis1.Tip = false;
                }
                servis1.VarisKod = varis[0];
                servis1.VarisHavalimani = varis[1];
                servis1.VarisSehir = varis[2];
                servis1.VarisUlke = varis[3];
                entities.SaveChanges();


                // 2.Ekstra Servis Bedeli güncellenecek
                var servis2 = entities.EkstraServis.Where(w => w.KalkisKod == servis1.VarisKod && w.VarisKod == servis1.KalkisKod).FirstOrDefault();
                servis2.BaslangicTarihi = Convert.ToDateTime(data["baslangicTarihi"]);
                servis2.BitisTarihi = Convert.ToDateTime(data["bitisTarihi"]);
                servis2.DonusEklensinMi = true;
                servis2.KalkisKod = varis[0];
                servis2.KalkisHavalimani = varis[1];
                servis2.KalkisSehir = varis[2];
                servis2.KalkisUlke = varis[3];
                servis2.KoltukLimiti = Convert.ToInt32(data["dkoltukLimiti"]);
                servis2.LimiteBagliServisBedeli = Convert.ToDecimal(data["dlimiteBagliServis"]);
                servis2.ServisBedeli = Convert.ToDecimal(data["dekstraServisBedeli"]);
                if (data["tip"] == "İÇ HAT")
                {
                    servis2.Tip = true;
                }
                else
                {
                    servis2.Tip = false;
                }
                servis2.VarisKod = kalkis[0];
                servis2.VarisHavalimani = kalkis[1];
                servis2.VarisSehir = kalkis[2];
                servis2.VarisUlke = kalkis[3];
                entities.SaveChanges();
            }
            else
            {
                var servis = entities.EkstraServis.Where(w => w.Id == Id).FirstOrDefault();
                // Tek yön Ekstra Servis Bedeli ekleme
                servis.BaslangicTarihi = Convert.ToDateTime(data["baslangicTarihi"]);
                servis.BitisTarihi = Convert.ToDateTime(data["bitisTarihi"]);
                servis.DonusEklensinMi = false;
                servis.KalkisKod = kalkis[0];
                servis.KalkisHavalimani = kalkis[1];
                servis.KalkisSehir = kalkis[2];
                servis.KalkisUlke = kalkis[3];

                servis.KoltukLimiti = Convert.ToInt32(data["koltukLimiti"]);
                servis.LimiteBagliServisBedeli = Convert.ToDecimal(data["limiteBagliServis"]);
                servis.ServisBedeli = Convert.ToDecimal(data["ekstraServisBedeli"]);
                if (data["tip"] == "İÇ HAT")
                {
                    servis.Tip = true;
                }
                else
                {
                    servis.Tip = false;
                }

                servis.VarisKod = varis[0];
                servis.VarisHavalimani = varis[1];
                servis.VarisSehir = varis[2];
                servis.VarisUlke = varis[3];
                entities.SaveChanges();

            }
            vm.ekstraServis = entities.EkstraServis.ToList();
            return PartialView(vm);
        }


        public JsonResult JsonGetir(string term)
        {
            string B = string.Join("", term.Normalize(NormalizationForm.FormD).Where(k => char.GetUnicodeCategory(k) != UnicodeCategory.NonSpacingMark));
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string response = "";
            var data = new List<AirportList>();

            using (StreamReader responseReader = new StreamReader(Server.MapPath("~/Scripts/airports-tr.json")))
            {
                response = responseReader.ReadToEnd();
                //var account = JsonConvert.DeserializeObject<List<AirportList>>(response);
                //var personlist = ser.Deserialize<List<AirportList>>(response);

            }
            AirportList airport = new AirportList();
            JObject JsonParser = JObject.Parse(response);
            JArray airportLsit = (JArray)JsonParser["result"];
            var array = airportLsit.ToObject<List<AirportList>>();



            var result = (from flight in array
                          where flight.cityCode.ToUpper().Contains(B.ToUpper())
                          || flight.cityName.ToUpper().Contains(B.ToUpper())
                          || flight.code.ToUpper().Contains(B.ToUpper())
                          || flight.name.ToUpper().Contains(B.ToUpper())
                          select new
                          {
                              label = flight.code + "-" + flight.name + "-" + flight.cityName + "-" + flight.countryName,
                              val = flight.name + "-" + flight.code
                          }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region YENİ SAYFA
        //[KullaniciYetkiler(Roles = "YeniSayfaGorme")]
        //[MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "YeniSayfa")]
        public ActionResult YeniSayfa(int? id)
        {
            if (id != null)
            {
                vm.sayfalar = entities.Sayfalar.Where(w => w.SayfaId == id).ToList();
            }
            else
            {
                vm.sayfalar = entities.Sayfalar.ToList();
            }

            return View(vm);
        }
        [HttpPost]
        public ActionResult YeniSayfa(int? id,FormCollection data)
        {
            if (id != null)
            {
                vm.sayfalar = entities.Sayfalar.Where(w => w.SayfaId == id).ToList();
            }
            else
            {
                vm.sayfalar = entities.Sayfalar.ToList();
            }

            return View(vm);
        }
        [HttpGet]
        public PartialViewResult SayfaKategoriEkle()
        {
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult SayfaKategoriEkle(FormCollection data)
        {
            return PartialView();
        }
        public class JsTreeModel
        {
            public string id { get; set; }
            public string text { get; set; }
            public Data data { get; set; }
            public IEnumerable<JsTreeModel> children { get; set; }
        }
        public class Data
        {
            public string multimedia { get; set; }
            public string edit { get; set; }
            public string delete { get; set; }
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "MultiMedia")]
        public ActionResult MultiMedia(int? SayfaId)
        {
            vm.sayfaMultiMedia = entities.SayfaMultiMedia.Where(w => w.SayfaId == SayfaId).ToList();
            ViewBag.SayfaId = SayfaId;
            return View(vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        public PartialViewResult SayfaMultimediaEkle(int? SayfaId)
        {
            ViewBag.SayfaId = SayfaId;
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult SayfaMultimediaEkle(HttpPostedFileBase resim, int? SayfaId)
        {
            string adres = "";
            SayfaMultiMedia multiMedia = new SayfaMultiMedia();
            if (resim != null)
            {
                if (resim.ContentLength > 0)
                {

                    string fileName = Path.GetFileNameWithoutExtension(resim.FileName);
                    fileName += "_" + Guid.NewGuid().ToString("N");
                    fileName += Path.GetExtension(resim.FileName);
                    var routhPath = "/Content/images";
                    var path = Path.Combine(routhPath, fileName);
                    resim.SaveAs(Server.MapPath(path));
                    adres = path;

                }
                multiMedia.Resim = adres;
                multiMedia.SayfaId = SayfaId;
                entities.SayfaMultiMedia.Add(multiMedia);
                entities.SaveChanges();
            }
            vm.sayfaMultiMedia = entities.SayfaMultiMedia.Where(w => w.SayfaId == SayfaId).ToList();
            return PartialView("MultiMedia", vm);
        }
        [MyFilterAdmin]
        [HttpGet]
        public PartialViewResult SayfaMultiMediaGuncelle(int? MultiMediaId)
        {
            vm.sayfaMultiMedia = entities.SayfaMultiMedia.Where(w => w.MultiMediaId == MultiMediaId);
            return PartialView(vm);
        }
        [HttpPost]
        public PartialViewResult SayfaMultiMediaGuncelle(HttpPostedFileBase resim, int? MultiMediaId)
        {
            string adres = "";
            var multiMedia = entities.SayfaMultiMedia.First(w => w.MultiMediaId == MultiMediaId);
            int SayfaId = Convert.ToInt32(multiMedia.SayfaId);

            if (resim != null)
            {
                if (resim.ContentLength > 0)
                {

                    string fileName = Path.GetFileNameWithoutExtension(resim.FileName);
                    fileName += "_" + Guid.NewGuid().ToString("N");
                    fileName += Path.GetExtension(resim.FileName);
                    var routhPath = "/Content/images";
                    var path = Path.Combine(routhPath, fileName);
                    resim.SaveAs(Server.MapPath(path));
                    adres = path;

                }
                multiMedia.Resim = adres;
                entities.SaveChanges();

            }
            vm.sayfaMultiMedia = entities.SayfaMultiMedia.Where(w => w.SayfaId == SayfaId).ToList();
            return PartialView("MultiMedia", vm);
        }

        public PartialViewResult SayfaMultiMediaSil(int? MultiMediaId)
        {
            var multiMedia = entities.SayfaMultiMedia.First(w => w.MultiMediaId == MultiMediaId);
            int SayfaId = Convert.ToInt32(multiMedia.SayfaId);
            entities.SayfaMultiMedia.Remove(entities.SayfaMultiMedia.First(s => s.MultiMediaId == MultiMediaId));
            entities.SaveChanges();
            vm.sayfaMultiMedia = entities.SayfaMultiMedia.Where(w => w.MultiMediaId == MultiMediaId).ToList();
            return PartialView("MultiMedia", vm);
        }

        public ActionResult Nodes()
        {
            //var nodes = new List<JsTreeModel>();
            var data = entities.Sayfalar.ToList();

            IEnumerable<JsTreeModel> nodes = data.RecursiveJoin(element => element.SayfaId,
               element => element.UstSayfaId,
               (Sayfalar element, IEnumerable<JsTreeModel> children) => new JsTreeModel()
               {
                   id = element.SayfaId.ToString(),
                   text = element.Ad,
                   children = children,
                   data = new Data()
                   {
                       edit = "<button class='btn btn-xs btn-success' id='btnSubmit' href = '/Yonetim/Panel/KategoriSil/" + element.SayfaId + "'><span> DÜZENLE </ span ></ button>",

                       delete = "<a class='btn btn-xs btn-danger' id='btnSubmit' href = 'MultiMedia/" + element.SayfaId + "'><span> SİL </ span ></ a>",

                       multimedia = "<a class='btn btn-xs btn-info' id='btnSubmit' href = '/Yonetim/Panel/MultiMedia?SayfaId=" + element.SayfaId + "'><span> MULTİMEDYA </ span ></ a>"
                   }
               }).ToList();


            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Kategoriler")]
        public ActionResult Kategoriler()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniSayfa(FormCollection data)
        {

            return View();
        }

        #endregion

        #region KUTU KATEGORİLERİ
        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "KutuKategorileriGorme")]
        public ActionResult KutuKategorileri()
        {
            vm.kutuKategori = entities.KutuKategori.ToList();
            return View(vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "KutuKategoriEkle")]
        public PartialViewResult KutuKategoriEkle()
        {
            return PartialView();
        }
        [HttpPost]
        [KullaniciYetkiler(Roles = "KutuKategoriEkle")]
        public PartialViewResult KutuKategoriEkle(FormCollection data)
        {
            KutuKategori kategori = new KutuKategori();
            kategori.Durum = true;
            kategori.Kategori = data["kutuKategori"];
            entities.KutuKategori.Add(kategori);
            entities.SaveChanges();
            vm.kutuKategori = entities.KutuKategori.ToList();
            return PartialView("KutuKategorileri", vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "KutuKategoriGuncelleme")]
        public PartialViewResult KutuKategoriGuncelle(int KategoriId)
        {
            vm.kutuKategori = entities.KutuKategori.Where(w => w.KategoriId == KategoriId);
            return PartialView(vm);
        }
        [HttpPost]
        [KullaniciYetkiler(Roles = "KutuKategoriGuncelleme")]
        public PartialViewResult KutuKategoriGuncelle(FormCollection data, int id)
        {
            var kategori = entities.KutuKategori.Where(w => w.KategoriId == id).FirstOrDefault();
            kategori.Kategori = data["kutuKategori"];
            entities.SaveChanges();
            vm.kutuKategori = entities.KutuKategori.ToList();
            return PartialView("KutuKategorileri", vm);
        }
        [KullaniciYetkiler(Roles = "KutuKategoriDurum")]
        public ActionResult KutuKategoriDurum(int? id)
        {
            var kategori = entities.KutuKategori.Where(w => w.KategoriId == id).FirstOrDefault();

            if (kategori.Durum == true)
            {
                kategori.Durum = false;
            }
            else
            {
                kategori.Durum = true;
            }
            entities.SaveChanges();
            vm.kutuKategori = entities.KutuKategori.ToList();

            return RedirectToAction("KutuKategorileri", vm);
        }

        #endregion

        #region KUTULAR

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Kutular")]
        public ActionResult Kutular(int? KategoriId)
        {
            if (KategoriId != null)
            {
                vm.kutu = entities.Kutu.Where(w => w.KategoriId == KategoriId).ToList();

            }
            else
            {
                vm.kutu = entities.Kutu.ToList();
            }
            vm.kutuKategori = entities.KutuKategori.ToList();
            return View(vm);
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "KutuEkle")]
        public PartialViewResult KutuEkle()
        {
            vm.kutuKategori = entities.KutuKategori.ToList();
            return PartialView(vm);
        }
        [HttpPost]
        public PartialViewResult KutuEkle(FormCollection data, HttpPostedFileBase resim)
        {
            string kalkisYeri = data["kalkisYeri"];
            string varisYeri = data["varisYeri"];
            string[] kalkis = kalkisYeri.Split('-');
            string[] varis = varisYeri.Split('-');
            if (data["kutuKategori"] != "")
            {
                Kutu kutu = new Kutu();
                string adres = "";
                if (resim != null)
                {
                    if (resim.ContentLength > 0)
                    {

                        string fileName = Path.GetFileNameWithoutExtension(resim.FileName);
                        fileName += "_" + Guid.NewGuid().ToString("N");
                        fileName += Path.GetExtension(resim.FileName);
                        var routhPath = "/Content/images";
                        var path = Path.Combine(routhPath, fileName);
                        resim.SaveAs(Server.MapPath(path));
                        adres = path;
                        //  adres = path.Replace(@"C:\Users\Metin\Desktop\projeler\KeyifliBiletPanel\KeyifliBiletPanel", "");

                    }
                    kutu.Resim = adres;
                }
                kutu.Durum = true;
                kutu.KategoriId = Convert.ToInt32(data["kutuKategori"]);
                //kutu.ARR_Airport_CityName = data["kalkisYeri"];
                //kutu.VarisYeri = data["varisYeri"];
                kutu.IsOneWay = true;
                kutu.ARR_Airport_Code = varis[0];
                kutu.ARR_Airport_Name = varis[1];
                kutu.ARR_Airport_CityName = varis[2];
                kutu.ARR_Airport_IsCity = false;

                kutu.DEP_Airport_Code = kalkis[0];
                kutu.DEP_Airport_Name = kalkis[1];
                kutu.DEP_Airport_CityName = kalkis[2];
                kutu.DEP_Airport_IsCity = false;
                entities.Kutu.Add(kutu);
                entities.SaveChanges();
            }

            vm.kutuKategori = entities.KutuKategori.ToList();
            vm.kutu = entities.Kutu.ToList();
            return PartialView("Kutular", vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "KutuGuncelle")]
        public PartialViewResult KutuGuncelle(int KutuId)
        {
            vm.kutu = entities.Kutu.Where(w => w.KutuId == KutuId);
            vm.kutuKategori = entities.KutuKategori.ToList();
            return PartialView(vm);
        }
        [HttpPost]
        public PartialViewResult KutuGuncelle(FormCollection data, HttpPostedFileBase resim, int id)
        {
            string kalkisYeri = data["kalkisYeri"];
            string varisYeri = data["varisYeri"];
            string[] kalkis = kalkisYeri.Split('-');
            string[] varis = varisYeri.Split('-');
            if (data["kutuKategori"] != "")
            {
                var kutu = entities.Kutu.Where(w => w.KutuId == id).FirstOrDefault();
                string adres = "";
                if (resim != null)
                {
                    if (resim.ContentLength > 0)
                    {

                        string fileName = Path.GetFileNameWithoutExtension(resim.FileName);
                        fileName += "_" + Guid.NewGuid().ToString("N");
                        fileName += Path.GetExtension(resim.FileName);
                        var routhPath = Server.MapPath("/Content/images");
                        var path = Path.Combine(routhPath, fileName);
                        resim.SaveAs(Server.MapPath(path));
                        adres = path;

                    }
                    kutu.Resim = adres;
                }

                kutu.KategoriId = Convert.ToInt32(data["kutuKategori"]);
                //kutu.ARR_Airport_CityName = data["kalkisYeri"];
                //kutu.VarisYeri = data["varisYeri"];
                kutu.IsOneWay = true;
                kutu.ARR_Airport_Code = varis[0];
                kutu.ARR_Airport_Name = varis[1];
                kutu.ARR_Airport_CityName = varis[2];
                kutu.ARR_Airport_IsCity = false;

                kutu.DEP_Airport_Code = kalkis[0];
                kutu.DEP_Airport_Name = kalkis[1];
                kutu.DEP_Airport_CityName = kalkis[2];
                kutu.DEP_Airport_IsCity = false;

                entities.SaveChanges();

            }
            vm.kutuKategori = entities.KutuKategori.ToList();
            vm.kutu = entities.Kutu.ToList();
            return PartialView("Kutular", vm);
        }
        [KullaniciYetkiler(Roles = "KutuDurum")]
        public ActionResult KutuDurum(int? id)
        {
            var kutu = entities.Kutu.Where(w => w.KutuId == id).FirstOrDefault();

            if (kutu.Durum == true)
            {
                kutu.Durum = false;
            }
            else
            {
                kutu.Durum = true;
            }
            entities.SaveChanges();
            vm.kutu = entities.Kutu.ToList();
            vm.kutuKategori = entities.KutuKategori.ToList();

            return RedirectToAction("Kutular", vm);
        }

        #endregion

        #region İLETİŞİM FORMU

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "IletisimFormu")]
        public ActionResult IletisimFormu()
        {
            vm.iletisimFormu = entities.IletisimFormu.ToList();
            return View(vm);
        }

        [KullaniciYetkiler(Roles = "MesajDurum")]
        public ActionResult MesajDurum(int? id)
        {
            var mesaj = entities.IletisimFormu.Where(w => w.IletisimId == id).FirstOrDefault();

            if (mesaj.Durum == true)
            {
                mesaj.Durum = false;

            }
            else
            {
                mesaj.Durum = true;
                mesaj.OkunmaTarihi = DateTime.Now;
            }
            entities.SaveChanges();
            vm.iletisimFormu = entities.IletisimFormu.ToList();


            return RedirectToAction("IletisimFormu", vm);
        }

        #endregion

        #region E-BÜLTEN

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Bulten")]
        public ActionResult Bulten()
        {
            vm.eBulten = entities.EBulten.ToList();
            return View(vm);
        }

        [KullaniciYetkiler(Roles = "BultenDurum")]
        public ActionResult BultenDurum(int? id)
        {
            var bulten = entities.EBulten.Where(w => w.BultenId == id).FirstOrDefault();

            if (bulten.Durum == true)
            {
                bulten.Durum = false;

            }
            else
            {
                bulten.Durum = true;

            }
            entities.SaveChanges();
            vm.eBulten = entities.EBulten.ToList();


            return RedirectToAction("Bulten", vm);
        }

        #endregion

        #region SİSTEM

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Sistem")]
        public ActionResult Sistem()
        {
            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return View(vm);
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Tanimlar")]
        public ActionResult Tanimlar(int? id, FormCollection data)
        {
            var tanim = entities.Tanimlar.Where(w => w.TanimId == id).FirstOrDefault();
            tanim.Adres = data["adres"];
            tanim.Fax = data["fax"];
            tanim.Harita = data["harita"];
            tanim.Mail = data["mail"];
            tanim.SiteAdi = data["siteAdi"];
            tanim.Telefon = data["telefon"];
            tanim.Url = data["url"];
            entities.SaveChanges();
            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return View("Sistem", vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "KullaniciEkle")]
        public PartialViewResult KullaniciEkle()
        {
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult KullaniciEkle(FormCollection data)
        {
            //Kullanici Ekleme
            var mail2 = data["mail"];
            int mail = entities.Kullanicilar.Where(w => w.Mail == mail2).Count();
            if (mail == 0)
            {
                Kullanicilar kullanici = new Kullanicilar();
                kullanici.Ad = data["ad"];
                kullanici.Durum = true;
                kullanici.KayitTarihi = DateTime.Now;
                kullanici.Mail = data["mail"];
                kullanici.Sifre = data["sifre"];
                kullanici.Soyad = data["soyad"];
                entities.Kullanicilar.Add(kullanici);
                entities.SaveChanges();

                int kullaniciId = entities.Kullanicilar.Where(w => w.Mail == mail2).FirstOrDefault().KullaniciId;

                //Kullanıcıya ait Rolü Ekleme
                Rol rol = new Rol();
                rol.KullaniciId = kullaniciId;
                rol.RolAdi = data["rol"];
                entities.Rol.Add(rol);
                entities.SaveChanges();
            }
            else
            {
                //Kullanıcı var 
            }


            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return PartialView("Sistem", vm);
        }

        [KullaniciYetkiler(Roles = "KullaniciDurum")]
        public ActionResult KullaniciDurum(int? id)
        {
            var kullanici = entities.Kullanicilar.Where(w => w.KullaniciId == id).FirstOrDefault();

            if (kullanici.Durum == true)
            {
                kullanici.Durum = false;

            }
            else
            {
                kullanici.Durum = true;

            }
            entities.SaveChanges();
            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return RedirectToAction("Sistem", vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "KullaniciGuncelle")]
        public PartialViewResult KullaniciGuncelle(int KullaniciId)
        {
            vm.kullanicilar = entities.Kullanicilar.Where(w => w.KullaniciId == KullaniciId);
            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult KullaniciGuncelle(FormCollection data, int id)
        {
            //Kullanici Düzenleme

            var kullanici = entities.Kullanicilar.Where(w => w.KullaniciId == id).FirstOrDefault();
            kullanici.Ad = data["ad"];
            kullanici.Mail = data["mail"];
            kullanici.Sifre = data["sifre"];
            kullanici.Soyad = data["soyad"];
            entities.SaveChanges();
            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return PartialView("Sistem", vm);
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "KullanıcıYetkilendirme")]
        public PartialViewResult KullanıcıYetkilendirme(int KullaniciId)
        {

            var data = entities.Rol.Where(w => w.KullaniciId == KullaniciId).FirstOrDefault();
            int rolId = data.RolId;
            vm.kullanicilar = entities.Kullanicilar.Where(w => w.KullaniciId == KullaniciId);
            vm.rol = entities.Rol.Where(w => w.KullaniciId == KullaniciId);
            vm.yetki = entities.Yetki.Where(w => w.RolId == rolId).ToList();
            vm.yetkiler = entities.Yetkiler;

            return PartialView(vm);

        }

        [HttpPost]
        public PartialViewResult KullanıcıYetkilendirme(int id, FormCollection form, string[] yetkiler, string[] yetkiKaldir)
        {
            int rolId = 0;
            string yetkiler2 = "";
            string yetkiler3 = "";
            var kullanici = entities.Kullanicilar.Where(w => w.KullaniciId == id).FirstOrDefault();
            //Yetki Güncelle
            rolId = entities.Rol.Where(w => w.KullaniciId == id).FirstOrDefault().RolId;
            var yetki = entities.Yetki.Where(w => w.RolId == rolId).ToList();
            int yetkiSayisi = entities.Yetki.Where(w => w.RolId == rolId).Count();

            //Yetki Kaldırma
            if (yetkiKaldir != null)
            {
                if (yetkiKaldir.Length >= 0)
                {
                    if (yetkiSayisi > yetkiKaldir.Length)
                    {
                        //Yetki Sil
                        entities.Yetki.RemoveRange(entities.Yetki.Where(x => x.RolId == rolId));
                        entities.SaveChanges();
                        for (int i = 0; i < yetkiKaldir.Length; i++)
                        {
                            yetkiler3 = yetkiKaldir[i];
                            Yetki yet = new Yetki();
                            yet.RolId = rolId;
                            yet.MetodAdi = yetkiler3;
                            entities.Yetki.Add(yet);
                            entities.SaveChanges();

                        }

                    }
                }


            }
            else
            {
                entities.Yetki.RemoveRange(entities.Yetki.Where(x => x.RolId == rolId));
                entities.SaveChanges();
            }
            if (yetkiler != null)
            {
                if (yetkiler.Length != 0)
                {
                    if (yetkiSayisi > 0)
                    {
                        // Yetki Ekleme
                        for (int i = 0; i < yetkiler.Length; i++)
                        {
                            yetkiler2 = yetkiler[i];
                            foreach (var item in yetki)
                            {
                                int query = entities.Yetki.Where(w => w.RolId == rolId && w.MetodAdi == yetkiler2).Count();
                                if (query == 0)
                                {
                                    if (yetkiler2 != item.MetodAdi)
                                    {
                                        Yetki yet = new Yetki();
                                        yet.RolId = rolId;
                                        yet.MetodAdi = yetkiler2;
                                        entities.Yetki.Add(yet);
                                        entities.SaveChanges();
                                    }
                                }


                            }

                        }
                    }
                    else
                    {
                        for (int i = 0; i < yetkiler.Length; i++)
                        {
                            yetkiler2 = yetkiler[i];
                            Yetki yet = new Yetki();
                            yet.RolId = rolId;
                            yet.MetodAdi = yetkiler2;
                            entities.Yetki.Add(yet);
                            entities.SaveChanges();
                        }
                    }

                }
            }

            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return PartialView("Sistem", vm);

        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "TakipKoduEkle")]
        public PartialViewResult TakipKoduEkle()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult TakipKoduEkle(FormCollection data)
        {
            TakipKodlari kod = new TakipKodlari();

            if (data["lokasyon"] != null)
            {
                kod.Lokasyon = true;
            }
            else
            {
                kod.Lokasyon = false;
            }
            kod.Durum = true;
            kod.Script = data["script"];
            kod.Servis = data["servis"];
            entities.TakipKodlari.Add(kod);
            entities.SaveChanges();

            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return PartialView("Sistem", vm);
        }

        [KullaniciYetkiler(Roles = "TakipDurum")]
        public ActionResult TakipDurum(int? id)
        {
            var takip = entities.TakipKodlari.Where(w => w.TakipId == id).FirstOrDefault();

            if (takip.Durum == true)
            {
                takip.Durum = false;

            }
            else
            {
                takip.Durum = true;

            }
            entities.SaveChanges();
            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return RedirectToAction("Sistem", vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "TakipDuzenle")]
        public PartialViewResult TakipDuzenle(int TakipId)
        {
            vm.takipKodlari = entities.TakipKodlari.Where(w => w.TakipId == TakipId);
            return PartialView(vm);
        }
        [HttpPost]
        public PartialViewResult TakipDuzenle(FormCollection data, int id)
        {
            //Kullanici Düzenleme

            var takip = entities.TakipKodlari.Where(w => w.TakipId == id).FirstOrDefault();
            if (data["lokasyon"] != null)
            {
                takip.Lokasyon = true;
            }
            else
            {
                takip.Lokasyon = false;
            }
            takip.Script = data["script"];
            takip.Servis = data["servis"];
            entities.SaveChanges();
            vm.takipKodlari = entities.TakipKodlari;
            vm.tanimlar = entities.Tanimlar;
            vm.kullanicilar = entities.Kullanicilar;
            return PartialView("Sistem", vm);
        }
        #endregion

        #region SOSYAL MEDYA

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "SosyalMedya")]
        public ActionResult SosyalMedya()
        {
            vm.sosyalMedya = entities.SosyalMedya.ToList();
            return View(vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "SosyalMedyaEkle")]
        public PartialViewResult SosyalMedyaEkle()
        {
            return PartialView();
        }
        [HttpPost]

        public PartialViewResult SosyalMedyaEkle(FormCollection data)
        {
            SosyalMedya sosyal = new SosyalMedya();
            sosyal.Durum = true;
            sosyal.Platform = data["platform"];
            sosyal.Url = data["url"];
            entities.SosyalMedya.Add(sosyal);
            entities.SaveChanges();
            vm.sosyalMedya = entities.SosyalMedya.ToList();
            return PartialView("SosyalMedya", vm);
        }

        [KullaniciYetkiler(Roles = "SosyalDurum")]
        public ActionResult SosyalDurum(int? id)
        {
            var sosyal = entities.SosyalMedya.Where(w => w.Id == id).FirstOrDefault();

            if (sosyal.Durum == true)
            {
                sosyal.Durum = false;
            }
            else
            {
                sosyal.Durum = true;
            }
            entities.SaveChanges();
            vm.sosyalMedya = entities.SosyalMedya.ToList();
            return RedirectToAction("SosyalMedya", vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "SosyalMedyaDuzenle")]
        public PartialViewResult SosyalMedyaDuzenle(int Id)
        {
            vm.sosyalMedya = entities.SosyalMedya.Where(w => w.Id == Id);
            return PartialView(vm);
        }
        [HttpPost]
        public PartialViewResult SosyalMedyaDuzenle(FormCollection data, int id)
        {
            //Kullanici Düzenleme

            var sosyal = entities.SosyalMedya.Where(w => w.Id == id).FirstOrDefault();
            sosyal.Platform = data["platform"];
            sosyal.Url = data["url"];
            entities.SaveChanges();
            vm.sosyalMedya = entities.SosyalMedya.ToList();
            return PartialView("SosyalMedya", vm);
        }

        #endregion

        #region SINIFLARA GÖRE İNDİRİM

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "Siniflar")]
        public ActionResult Siniflar()
        {
            vm.sinif = entities.Siniflar;
            return View(vm);
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "SinifEkle")]
        public PartialViewResult SinifEkle()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult SinifEkle(FormCollection data)
        {
            Siniflar sinif = new Siniflar();

            sinif.SinifAdi = data["sinif"];
            sinif.ServisBedeli = Convert.ToDecimal(data["servisBedeli"]);
            entities.Siniflar.Add(sinif);
            entities.SaveChanges();
            vm.sinif = entities.Siniflar.ToList();
            return PartialView("Siniflar", vm);
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "SinifGuncelle")]
        [HttpGet]
        public PartialViewResult SinifGuncelle(int SinifId)
        {
            vm.sinif = entities.Siniflar.Where(w => w.SinifId == SinifId);
            return PartialView(vm);
        }
        [HttpPost]
        public PartialViewResult SinifGuncelle(FormCollection data, int id)
        {
            //Kullanici Düzenleme

            var sinif = entities.Siniflar.Where(w => w.SinifId == id).FirstOrDefault();
            sinif.SinifAdi = data["sinif"];
            sinif.ServisBedeli = Convert.ToDecimal(data["servisBedeli"]);
            entities.SaveChanges();
            vm.sinif = entities.Siniflar.ToList();
            return PartialView("Siniflar", vm);
        }
        #endregion


        #region Mail Panel

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "MailPanel")]
        public ActionResult MailPanel()
        {
            vm.mail = entities.Mailler.ToList();
            return View(vm);
        }

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "MailEkle")]
        public PartialViewResult MailEkle()
        {
            vm.mailKategori = entities.MailKategori;
            vm.sinif = entities.Siniflar;
            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult MailEkle(FormCollection data)
        {
            Mailler mail = new Mailler();
            mail.MailKategoriId = Convert.ToInt32(data["kategori"]);
            mail.Baslik = data["baslik"];
            mail.Icerik = data["icerik"];
            mail.Durum = false;
            mail.Mail = data["mail"];
            mail.Port = Convert.ToInt32(data["port"]);
            mail.Sifre = data["sifre"];
            mail.SMTPAdresi = data["adres"];
            entities.Mailler.Add(mail);
            entities.SaveChanges();
            vm.mail = entities.Mailler.ToList();
            return PartialView("MailPanel", vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "MailGuncelle")]
        public PartialViewResult MailGuncelle(int MailId)
        {
            vm.mail = entities.Mailler.Where(w => w.MailId == MailId);
            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult MailGuncelle(FormCollection data, int id)
        {
            //Kullanici Düzenleme

            var mail = entities.Mailler.Where(w => w.MailId == id).FirstOrDefault();
            mail.Baslik = data["baslik"];
            mail.Icerik = data["icerik"];
            mail.Durum = false;
            mail.Mail = data["mail"];
            mail.Port = Convert.ToInt32(data["port"]);
            mail.Sifre = data["sifre"];
            mail.SMTPAdresi = data["adres"];
            entities.Mailler.Add(mail);
            entities.SaveChanges();
            vm.mail = entities.Mailler.ToList();
            return PartialView("MailPanel", vm);
        }

        [KullaniciYetkiler(Roles = "MailDurum")]
        public ActionResult MailDurum(int? id)
        {
            var mail = entities.Mailler.Where(w => w.MailId == id).FirstOrDefault();

            if (mail.Durum == true)
            {
                mail.Durum = false;
            }
            else
            {
                mail.Durum = true;
            }
            entities.SaveChanges();
            vm.mail = entities.Mailler.ToList();
            return PartialView("MailPanel", vm);
        }
        #endregion

        #region Mail Kategori

        //[MyFilterAdmin]
        //[KullaniciYetkiler(Roles = "MailPanel")]
        public ActionResult MailKategori()
        {
            vm.mailKategori = entities.MailKategori.ToList();
            return View(vm);
        }

        //[MyFilterAdmin]
       
        public PartialViewResult MailKategoriEkle()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult MailKategoriEkle(FormCollection data)
        {
           MailKategori mail = new MailKategori();

            mail.Aciklama = data["aciklama"];
            mail.Kod = data["kod"];
            entities.MailKategori.Add(mail);
            entities.SaveChanges();
            vm.mailKategori = entities.MailKategori.ToList();
            return PartialView("MailKategori", vm);
        }

        //[MyFilterAdmin]
        [HttpGet]
      
        public PartialViewResult MailKategoriGuncelle(int MailKategoriId)
        {
            vm.mailKategori = entities.MailKategori.Where(w => w.MailKategoriId == MailKategoriId);
            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult MailKategoriGuncelle(FormCollection data, int id)
        {
           
            var mail = entities.MailKategori.Where(w => w.MailKategoriId == id).FirstOrDefault();
            mail.Aciklama = data["aciklama"];
            mail.Kod = data["kod"];
            entities.SaveChanges();
            vm.mailKategori = entities.MailKategori;
            return PartialView("MailKategori", vm);
        }


        #endregion

        #region Sms Kategori

        //[MyFilterAdmin]
       // [KullaniciYetkiler(Roles = "MailPanel")]
        public ActionResult SmsKategori()
        {
            vm.smsKategori = entities.SmsKategori.ToList();
            return View(vm);
        }

        //[MyFilterAdmin]

        public PartialViewResult SmsKategoriEkle()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult SmsKategoriEkle(FormCollection data)
        {
            SmsKategori mail = new SmsKategori();

            mail.Aciklama = data["aciklama"];
            mail.Kod = data["kod"];
            entities.SmsKategori.Add(mail);
            entities.SaveChanges();
            vm.smsKategori = entities.SmsKategori.ToList();
            return PartialView("SmsKategori", vm);
        }

        //[MyFilterAdmin]
        [HttpGet]

        public PartialViewResult SmsKategoriGuncelle(int SmsKategoriId)
        {
            vm.smsKategori = entities.SmsKategori.Where(w => w.SmsKategoriId == SmsKategoriId);
            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult SmsKategoriGuncelle(FormCollection data, int id)
        {

            var sms = entities.SmsKategori.Where(w => w.SmsKategoriId == id).FirstOrDefault();
            sms.Aciklama = data["aciklama"];
            sms.Kod = data["kod"];
            entities.SaveChanges();
            vm.smsKategori = entities.SmsKategori;
            return PartialView("SmsKategori", vm);
        }


        #endregion

        #region SMS Panel

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "SmsPanel")]
        public ActionResult SmsPanel()
        {
            vm.sms = entities.Sms.ToList();
            return View(vm);
        }


        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "SmsEkle")]
        public PartialViewResult SmsEkle()
        {
            vm.smsKategori = entities.SmsKategori;
            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult SmsEkle(FormCollection data)
        {
            Sms sms = new Sms();
            sms.SmsKategoriId = Convert.ToInt32(data["kategori"]);
            sms.ApiAdresi = data["api"];
            sms.Durum = false;
            sms.KaynakAdres = data["kaynak"];
            sms.KullaniciAdi = data["kullanici"];
            sms.Mesaj = data["mesaj"];
            sms.Sifre = data["sifre"];

            entities.Sms.Add(sms);
            entities.SaveChanges();
            vm.sms = entities.Sms.ToList();
            return PartialView("SmsPanel", vm);
        }

        [MyFilterAdmin]
        [HttpGet]
        [KullaniciYetkiler(Roles = "SmsGuncelle")]
        public PartialViewResult SmsGuncelle(int SmsId)
        {
            vm.sms = entities.Sms.Where(w => w.SmsId == SmsId);

            return PartialView(vm);
        }

        [HttpPost]
        public PartialViewResult SmsGuncelle(FormCollection data, int id)
        {
            var sms = entities.Sms.Where(w => w.SmsId == id).FirstOrDefault();
            sms.ApiAdresi = data["api"];
            sms.KaynakAdres = data["kaynak"];
            sms.KullaniciAdi = data["kullanici"];
            sms.Mesaj = data["mesaj"];
            sms.Sifre = data["sifre"];
            entities.SaveChanges();
            vm.sms = entities.Sms.ToList();
            return PartialView("SmsPanel", vm);
        }

        [KullaniciYetkiler(Roles = "SmsDurum")]
        public ActionResult SmsDurum(int? id)
        {
            var sms = entities.Sms.Where(w => w.SmsId == id).FirstOrDefault();

            if (sms.Durum == true)
            {
                sms.Durum = false;
            }
            else
            {
                sms.Durum = true;
            }
            entities.SaveChanges();
            vm.sms = entities.Sms.ToList();
            return PartialView("SmsPanel", vm);
        }
        #endregion

        #region DEĞİŞEN BİLETLER LİSTESİ

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "DegisenBiletlerListesi")]
        public async System.Threading.Tasks.Task<ActionResult> DegisenBiletlerListesi()
        {
            var res = await Iati.Search.GetChangedOrders();
            return View(res);
        }
        #endregion
        #region CHARTER UÇUŞLAR LİSTESİ
        GetSpecialOffersRequest model = new GetSpecialOffersRequest();

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "CharterUcuslarListesi")]
        public async System.Threading.Tasks.Task<ActionResult> CharterUcuslarListesi()
        {
            var res = await Iati.Search.GetSpecialOffers();
            return View(res);
        }
        #endregion
        #region RAPORLAR

        [MyFilterAdmin]
        [KullaniciYetkiler(Roles = "SatisListesi")]
        public ActionResult SatisListesi()
        {
            vm.passenger = entities.passengers;
            vm.orderResult = entities.OrderResult;
            vm.booking = entities.BookingInfo;
            vm.leg = entities.legs;
            vm.uyeler = entities.Uyeler;
            return View(vm);
        }
        [MyFilterAdmin]
        public ActionResult KomisyonRapolari()
        {
            vm.passenger = entities.passengers;
            vm.orderResult = entities.OrderResult;
            vm.booking = entities.BookingInfo;
            vm.leg = entities.legs;
            vm.uyeler = entities.Uyeler;
            return View(vm);
        }
        [MyFilterAdmin]
        [HttpGet]
        public ActionResult BiletDetay(int Id)
        {
            vm.orderResult = entities.OrderResult.Where(w => w.Id == Id).ToList();
            return View(vm);
        }

        #endregion
        #region GİRİŞ BÖLÜMÜ
        public ActionResult Login()
        {
            if (id != 0)
            {

                Session["KullaniciId"] = id;
                return RedirectToAction("Index", "Panel", new { Id = id });

            }
            else
            {
                //TempData["Message"] = "Hatalı Giriş. Lutfen Kontrol Edip Tekrar Deneyiniz.";
                return View();
            }

        }


        [HttpPost]
        public ActionResult Login(FormCollection form)
        {

            string kullaniciadi = form["username"];
            string sifre = form["password"];
            id = entities.Kullanicilar.Where(x => x.Mail == kullaniciadi && x.Sifre == sifre).FirstOrDefault().KullaniciId;
            try
            {

                if (sifre != null && (kullaniciadi != null && (kullaniciadi.Trim() == kullaniciadi && sifre.Trim() == sifre))) // Kullanıcı var
                {
                    int userId = id; // Login olan Kullanıcıya ait kullanici ID
                    var query = entities.Rol.Where(w => w.KullaniciId == userId).FirstOrDefault();
                    // Kullanıcı Admin Rolüne sahip
                    string roles = query.RolAdi;

                    string userData = userId.ToString(CultureInfo.InvariantCulture) + "," + kullaniciadi.Trim() + "," + roles;
                    // virgül ile ayırarak önce ID sonra kullanıcı adını KullanıcıDatası olarak cookiye atacağız.


                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                      1,                                     // Ticket Versiyonu
                      kullaniciadi,                              // Kullanıcı Adı
                      DateTime.Now,                          // Düzenlenme Tarihi
                      DateTime.Now.AddMinutes(120),           // Cookie Bitiş Süresi
                      false,                          // true to persist across browser sessions
                      userData,                              // Kullanıcı Datası (Id vs..) virgül ile ayırarak hepsine ulaşıyoruz
                      FormsAuthentication.FormsCookiePath);  // Cookie Path

                    // Ticket'ı makina dilinde şifreliyoruz
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    // Add the cookie to the request to save it
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie); // Cookieyi ekliyoruz

                    //  return Json(true, JsonRequestBehavior.AllowGet); // Kullanıcı Var
                    return RedirectToAction("Index", "Panel", new { Id = id });
                }
                return Json(null, JsonRequestBehavior.AllowGet); // Kullanıcı Bulunamadı


            }
            catch (Exception)
            {
                TempData["Message"] = "Hatalı Giriş. Lutfen Kontrol Edip Tekrar Deneyiniz.";
                return View();
            }
        }
        #endregion
        public ActionResult ErişimEngeli(string rolAdi)
        {
            return View();
        }
    }
}