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
    
    public partial class Uyeler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uyeler()
        {
            this.OrderResult = new HashSet<OrderResult>();
        }
    
        public int UyeId { get; set; }
        public string Mail { get; set; }
        public string GSM { get; set; }
        public string Sifre { get; set; }
        public string AdSoyad { get; set; }
        public Nullable<bool> KampanyaDurum { get; set; }
        public Nullable<bool> Durum { get; set; }
        public Nullable<System.DateTime> KayitTarihi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderResult> OrderResult { get; set; }
    }
}
