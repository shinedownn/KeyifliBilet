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
    
    public partial class Menuler
    {
        public Nullable<int> TipId { get; set; }
        public int MenuId { get; set; }
        public string Sira { get; set; }
        public string Tip { get; set; }
        public Nullable<int> SayfaId { get; set; }
    
        public virtual MenuTipleri MenuTipleri { get; set; }
    }
}
