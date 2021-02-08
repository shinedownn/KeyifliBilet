using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyifliBilet.Areas.Yonetim.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Models.Uyeler> uyeler { get; set; }
        public IEnumerable<Models.Kullanicilar> kullanicilar { get; set; }
        public IEnumerable<Models.Rol> rol { get; set; }
        public IEnumerable<Models.Yetki> yetki { get; set; }
        public IEnumerable<Models.EkstraServis> ekstraServis { get; set; }
        public IEnumerable<Models.Sms> sms { get; set; }
        public IEnumerable<Models.SayfaMultiMedia> sayfaMultiMedia { get; set; }
        public IEnumerable<Models.Mailler> mail { get; set; }
        public IEnumerable<Models.KutuKategori> kutuKategori { get; set; }
        public IEnumerable<Models.Kutu> kutu { get; set; }
        public IEnumerable<Models.Sayfalar> sayfalar { get; set; }
        public IEnumerable<Models.SayfaIcerigi> sayfaIcerigi { get; set; }
        public IEnumerable<Models.IletisimFormu> iletisimFormu { get; set; }
        public IEnumerable<Models.EBulten> eBulten { get; set; }
        public IEnumerable<Models.Tanimlar> tanimlar { get; set; }
        public IEnumerable<Models.TakipKodlari> takipKodlari { get; set; }
        public IEnumerable<Models.Yetkiler> yetkiler { get; set; }
        public IEnumerable<Models.SosyalMedya> sosyalMedya { get; set; }
        public IEnumerable<Models.Siniflar> sinif { get; set; }
        public IEnumerable<Models.OrderResult> orderResult { get; set; }
        public IEnumerable<Models.passengers> passenger { get; set; }
        public IEnumerable<Models.MailKategori> mailKategori { get; set; }
        public IEnumerable<Models.SmsKategori> smsKategori { get; set; }
        public IEnumerable<Models.legs> leg { get; set; }
        public IEnumerable<Models.BookingInfo> booking { get; set; }

    }
}