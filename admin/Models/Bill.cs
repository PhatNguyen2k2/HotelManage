//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelManage.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bill
    {
        public int id { get; set; }
        public int c_id { get; set; }
        public int e_id { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> b_date { get; set; }
        public Nullable<double> total { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}