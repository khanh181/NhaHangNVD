using LibData.Models;
using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static LibData.Config.CommonConfig;

namespace NhaHangNVD.Controllers
{
    public class DatChoController : Controller
    {
        [HttpPost]
        public JsonResult ThemDatCho(LibData.DatCho model)
        {
            var provider = new LibData.Provider.DatChos();
            try
            {
                if (model.NgayDen < DateTime.Now)
                {
                    return Json(new { status = 400, msg = "Thời gian đến không hợp lệ!" }, JsonRequestBehavior.AllowGet);
                }
                var gioDen = model.NgayDen.TimeOfDay;
                var item = new LibData.DatCho()
                {
                    HoTen = model.HoTen,
                    Email = model.Email,
                    SDT = model.SDT,
                    NgayDen = model.NgayDen,
                    ThoiGianDen = gioDen,
                    SoNguoi = model.SoNguoi,
                    YeuCau = model.YeuCau,
                    NgayTao = DateTime.Now,
                };
                var newId = provider.InsertButReturnId(item);
                if (newId != -1)
                {
                    return Json(new { status = 200, msg = "Đã tiếp nhận thông tin đặt chỗ nhân viên nhà hàng sẽ liên hệ tới quý khách, vui lòng chờ trong ít phút." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thông tin đặt chỗ sai, vui lòng xem lại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthenticationFilter]
        public ActionResult DanhSachDatCho()
        {
            var model = new LibData.Provider.DatChos().getAll();
            ViewData["listBanAn"] = new LibData.Provider.BanAns().getAllKhongHong();
            return View(model);
        }

        [CustomAuthenticationFilter]
        public ActionResult ListDatCho()
        {
            var model = new LibData.Provider.DatChos().getAll();
            return View(model);
        }

        [CustomAuthenticationFilter]
        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneDatCho(int id)
        {
            var provider = new LibData.Provider.DatChos();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "Đặt chỗ đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Từ chỗi thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Từ chỗi thất bại!" }, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthenticationFilter]
        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult TiepNhanDatCho(int DatChoId, int IdBanAn, bool askComfirm)
        {
            var provider_dc = new LibData.Provider.DatChos();
            var provider_banan = new LibData.Provider.BanAns();
            var provider_hoadon = new LibData.Provider.HoaDons();
            var provider_kh = new LibData.Provider.KhachHangs();
            try
            {
                var dc = provider_dc.getById(DatChoId);
                var ban = provider_banan.getById(IdBanAn);
                if (dc == null)
                {
                    return Json(new { status = 400, msg = "Đặt chỗ đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (ban.TinhTrang == (int)BanStatus.Ban && askComfirm)
                {
                    return Json(new { status = 402, msg = "Bàn được chọn hiện đang bận bạn có muốn tiếp tục?" }, JsonRequestBehavior.AllowGet);
                }
                if (ban.TinhTrang == (int)BanStatus.DuocDat && askComfirm)
                {
                    return Json(new { status = 403, msg = "Bàn được chọn hiện đang được đặt bạn có muốn tiếp tục?" }, JsonRequestBehavior.AllowGet);
                }
                var kh = provider_kh.getBySDT(dc.SDT);
                if (kh == null)
                {
                    var newkh = new LibData.KhachHang() {
                        TenKhach = dc.HoTen,
                        Email = dc.Email,
                        DienThoai = dc.SDT,
                        Password = "12345678",
                        NgayTao = DateTime.Now
                    };
                   var newid =  provider_kh.InsertButReturnId(newkh);
                   kh = provider_kh.getById(newid);
                }
                var newhd = new LibData.HoaDon()
                {
                    IdKhachHang = kh.Id,
                    IdBanAn = ban.Id,
                    ThoiGianDen = dc.NgayDen.Add(dc.ThoiGianDen),
                    SoLuongNguoi = dc.SoNguoi,
                    YeuCauDacBiet = dc.YeuCau,
                    TienKM = 0,
                    TongTien = 0,
                    TinhTrang = 0,
                    NgayTao = DateTime.Now
                };
                if (provider_hoadon.Insert(newhd))
                {
                    provider_dc.Delete(dc.Id);
                    ban.TinhTrang = (int)BanStatus.DuocDat;
                    ban.NgaySua = DateTime.Now;
                    provider_banan.Update(ban);
                    return Json(new { status = 200, msg = "Tiếp nhận thành công." }, JsonRequestBehavior.AllowGet);
                }    
                else
                {
                    
                    return Json(new { status = 400, msg = "Tiếp nhận thất bại." }, JsonRequestBehavior.AllowGet);
                }    
                    

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}