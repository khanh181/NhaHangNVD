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
    
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            this.NDHoaDon = new HashSet<NDHoaDon>();
            this.VnPayResult = new HashSet<VnPayResult>();
        }
    
        public int Id { get; set; }
        public int IdKhachHang { get; set; }
        public int IdBanAn { get; set; }
        public int SoLuongNguoi { get; set; }
        public System.DateTime ThoiGianDen { get; set; }
        public Nullable<System.DateTime> ThoiGianDi { get; set; }
        public string YeuCauDacBiet { get; set; }
        public string CodeKM { get; set; }
        public int TienKM { get; set; }
        public int TongTien { get; set; }
        public Nullable<int> HinhThucThanhToan { get; set; }
        public int TinhTrang { get; set; }
        public System.DateTime NgayTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
    
        public virtual BanAn BanAn { get; set; }
        public virtual KhachHang KhachHang { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NDHoaDon> NDHoaDon { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VnPayResult> VnPayResult { get; set; }
    }
}