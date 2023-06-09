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
    
    public partial class MonAn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MonAn()
        {
            this.CongThucMonAn = new HashSet<CongThucMonAn>();
            this.NDHoaDon = new HashSet<NDHoaDon>();
        }
    
        public int Id { get; set; }
        public int IdThucDon { get; set; }
        public string TenMon { get; set; }
        public string GioiThieu { get; set; }
        public string GioiThieuNgan { get; set; }
        public string HinhAnh { get; set; }
        public Nullable<bool> IsDacBiet { get; set; }
        public long GiaTien { get; set; }
        public System.DateTime NgayTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CongThucMonAn> CongThucMonAn { get; set; }
        public virtual ThucDon ThucDon { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NDHoaDon> NDHoaDon { get; set; }
    }
}
