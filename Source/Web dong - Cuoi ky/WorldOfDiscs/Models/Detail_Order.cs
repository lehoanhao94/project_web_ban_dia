//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorldOfDiscs.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Detail_Order
    {
        public int Id { get; set; }
        public Nullable<int> Id_Order { get; set; }
        public Nullable<int> Id_Disc { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
    
        public virtual Disc Disc { get; set; }
        public virtual Order Order { get; set; }
    }
}
