//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KeyifliBilet.Areas.Yonetim.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mailler
    {
        public int MailId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string SMTPAdresi { get; set; }
        public Nullable<int> Port { get; set; }
        public string Sifre { get; set; }
        public Nullable<bool> Durum { get; set; }
        public string Mail { get; set; }
        public int MailKategoriId { get; set; }
    
        public virtual MailKategori MailKategori { get; set; }
    }
}
