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
    
    public partial class Booking
    {
        public int r_id { get; set; }
        public int c_id { get; set; }
        public Nullable<System.DateTime> checkin { get; set; }
        public Nullable<System.DateTime> checkout { get; set; }
    
        public virtual Room Room { get; set; }
        public virtual Customer Customer { get; set; }
    }
}