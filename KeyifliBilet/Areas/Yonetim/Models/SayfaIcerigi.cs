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
    
    public partial class SayfaIcerigi
    {
        public Nullable<int> SayfaId { get; set; }
        public int IcerikId { get; set; }
        public string Ad { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string SeoBasligi { get; set; }
        public string KeyWords { get; set; }
        public string SeoAciklama { get; set; }
    
        public virtual Sayfalar Sayfalar { get; set; }
    }
}