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
    
    public partial class OrderResult
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderResult()
        {
            this.BookingInfo = new HashSet<BookingInfo>();
            this.legs = new HashSet<legs>();
            this.passengers = new HashSet<passengers>();
        }
    
        public int Id { get; set; }
        public Nullable<int> orderId { get; set; }
        public string provider { get; set; }
        public Nullable<System.DateTime> creationDate { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public string contactGsm { get; set; }
        public string contactEmail { get; set; }
        public Nullable<int> pax { get; set; }
        public Nullable<int> UyeId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingInfo> BookingInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<legs> legs { get; set; }
        public virtual Uyeler Uyeler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<passengers> passengers { get; set; }
    }
}
