//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibData
{
    using System;
    using System.Collections.Generic;
    
    public partial class VnPayResult
    {
        public System.Guid Id { get; set; }
        public int IdHoaDon { get; set; }
        public string ObjectResult { get; set; }
        public System.DateTime NgayTao { get; set; }
    
        public virtual HoaDon HoaDon { get; set; }
    }
}
