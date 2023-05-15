using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;


namespace LibData.Provider
{
    public class ThongKes : ApplicationDbContexts
    {

        public List<HoaDon> getAllHoaDonByThongKe(DateTime from, DateTime to, string type, int? idBan, string searchKeyword)
        {
            try
            {
                var HoaDons = ApplicationDbContext.HoaDon.Where(x => x.ThoiGianDen >= from && x.ThoiGianDen <= to
                                        && (!x.ThoiGianDi.HasValue || x.ThoiGianDi.Value >= from && x.ThoiGianDi.Value <= to)
                                        && (idBan.HasValue ? x.IdBanAn.Equals(idBan.Value) : true)
                                        && (type.Contains("GiamGia") ? x.TienKM > 0 : true)
                                        && (type.Contains("Cash") ? x.HinhThucThanhToan.Value.Equals((int)CommonConfig.HinhThucThanhToan.Cash) && x.HinhThucThanhToan.HasValue : true)
                                        && (type.Contains("VnPay") ? x.HinhThucThanhToan.Value.Equals((int)CommonConfig.HinhThucThanhToan.VnPay) && x.HinhThucThanhToan.HasValue : true)
                                        && (string.IsNullOrEmpty(searchKeyword) ? true : x.KhachHang.DienThoai.Contains(searchKeyword))
                                        ).OrderByDescending(x => x.NgayTao).ToList();
                if (HoaDons != null)
                {
                    return HoaDons;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public List<HoaDon> getAllHoaDonByBanAn(int IdBanAn)
            {
                try
                {
                    var listHoaDon = ApplicationDbContext.HoaDon.Where(x => x.IdBanAn == IdBanAn ).ToList();
                    if (listHoaDon != null)
                    {
                        return listHoaDon;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        
    }
}
