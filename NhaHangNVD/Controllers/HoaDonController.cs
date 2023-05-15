using LibData.Config;
using LibData.Models;
using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class HoaDonController : Controller
    {
        public ActionResult ModalBanDuocDat(int idBan)
        { 
            var model = new LibData.Provider.HoaDons().getAllHoaDonByIdBan(idBan);
            ViewData["listBan"] = new LibData.Provider.BanAns().getAllBanTrong();
            if (model == null)
            {
                return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (DateTime.Now < model[0].ThoiGianDen)
                    return View(model);
                else
                    return Json(new { status = 400, msg = "Bàn đã chuyển sang trạng thái khác." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ModalBanBan(int idBan)
        {
            var model = new LibData.Provider.HoaDons().getAllHoaDonByIdBan(idBan);
            ViewData["listBan"] = new LibData.Provider.BanAns().getAllBanTrong();
            ViewData["listMonAn"] = new LibData.Provider.MonAns().getAll();
            if (model == null)
            {
                return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (DateTime.Now >= model[0].ThoiGianDen)
                {
                    ViewData["listNDHoaDon"] = new LibData.Provider.NDHoaDons().getAllByIdHoaDon(model[0].Id);
                    return View(model);
                }  
                else
                    return Json(new { status = 400, msg = "Bàn đã chuyển sang trạng thái khác." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ChuyenBan(int id, int idBanChuyenDen)
        {
            var model = new LibData.Provider.HoaDons().getById(id);
            var banNew = new LibData.Provider.BanAns().getById(idBanChuyenDen);
            if (model == null)
            {
                return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if(banNew.TinhTrang != (int)CommonConfig.BanStatus.Trong)
                {
                    return Json(new { status = 400, msg = "Bàn chuyển đến không còn trống." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //var banOld = new LibData.Provider.BanAns().getById(model.IdBanAn);
                    model.IdBanAn = idBanChuyenDen;
                    if (new LibData.Provider.HoaDons().Update(model))
                    {
                        //banNew.TinhTrang = banOld.TinhTrang;
                        //new LibData.Provider.BanAns().Update(banNew);

                        /*var listHD_banOld = new LibData.Provider.HoaDons().getAllHoaDonByIdBan(banOld.Id);
                        if(listHD_banOld != null && listHD_banOld.Count > 0)
                        {
                            if (DateTime.Now < listHD_banOld[0].ThoiGianDen)
                                banOld.TinhTrang = (int)CommonConfig.BanStatus.DuocDat;
                            else if (DateTime.Now >= listHD_banOld[0].ThoiGianDen)
                                banOld.TinhTrang = (int)CommonConfig.BanStatus.Ban;
                        }   
                        else
                        {
                            banOld.TinhTrang = (int)CommonConfig.BanStatus.Trong;
                        }
                        new LibData.Provider.BanAns().Update(banOld);*/
                        return Json(new { status = 200, msg = "Chuyển thành công." }, JsonRequestBehavior.AllowGet);
                    }   
                    else
                    {
                        return Json(new { status = 400, msg = "Chuyển thất bại." }, JsonRequestBehavior.AllowGet);
                    }    
                }    
            }
        }

        public JsonResult HuyHoaDon(int Id)
        {
            var model = new LibData.Provider.HoaDons().getById(Id);
            if (model == null)
            {
                return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                model.TinhTrang = (int)CommonConfig.HoaDonStatus.Huy;
                if (new LibData.Provider.HoaDons().Update(model))
                {
                    return Json(new { status = 200, msg = "Hủy thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Hủy thất bại." }, JsonRequestBehavior.AllowGet);
                }
                
            }
        }

        public JsonResult EditHoaDon(LibData.HoaDon model)
        {
            var hd = new LibData.Provider.HoaDons().getById(model.Id);
            if (hd == null)
            {
                return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                hd.SoLuongNguoi = model.SoLuongNguoi;
                hd.YeuCauDacBiet = model.YeuCauDacBiet;
                hd.ThoiGianDen = model.ThoiGianDen;
                if (new LibData.Provider.HoaDons().Update(hd))
                {
                    return Json(new { status = 200, msg = "Thay đổi thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thay đổi thất bại." }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        public JsonResult AddNDHoaDon(LibData.NDHoaDon model)
        {
            var ma = new LibData.Provider.MonAns().getById(model.IdMonAn);
            if (ma == null)
            {
                return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (new LibData.Provider.NDHoaDons().checkexist(model.IdHoaDon, model.IdMonAn, -1))
                {
                    return Json(new { status = 400, msg = "Món ăn đã được thêm vào hóa đơn." }, JsonRequestBehavior.AllowGet);
                }
                var nd = new LibData.NDHoaDon()
                {
                    IdHoaDon = model.IdHoaDon,
                    IdMonAn = model.IdMonAn,
                    SoLuong = model.SoLuong,
                    Giatien = ma.GiaTien,
                    ThanhTien = ((int)(model.SoLuong * ma.GiaTien)),
                    NgayTao = DateTime.Now,
                };
                var newId = new LibData.Provider.NDHoaDons().InsertButReturnId(nd);
                if (newId != -1)
                {
                    return Json(new { status = 200, msg = "Gọi món thành công.", data = new { Id = newId, nd.IdMonAn, ma.TenMon, nd.SoLuong, Giatien = string.Format("{0:0,0 VNĐ}", nd.Giatien), ThanhTien = string.Format("{0:0,0 VNĐ}", nd.ThanhTien), TongTien = string.Format("{0:0,0 VNĐ}", new LibData.Provider.NDHoaDons().getTongTienTamTinh(model.IdHoaDon)) } }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Gọi món thất bại." }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        public JsonResult EditNDHoaDon(LibData.NDHoaDon model)
        {
            var hd = new LibData.Provider.NDHoaDons().getById(model.Id);
            if (hd == null)
            {
                return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                hd.SoLuong = model.SoLuong;
                hd.ThanhTien = ((int)(model.SoLuong * hd.Giatien));
                if (new LibData.Provider.NDHoaDons().Update(hd))
                {
                    return Json(new { status = 200, msg = "Sửa gọi món thành công.", data = new {hd.Id, ThanhTien = string.Format("{0:0,0 VNĐ}", hd.ThanhTien), TongTien = string.Format("{0:0,0 VNĐ}", new LibData.Provider.NDHoaDons().getTongTienTamTinh(hd.IdHoaDon)) } }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Sửa gọi món thất bại." }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        public JsonResult XoaNDHoaDon(int id)
        {
            var hd = new LibData.Provider.NDHoaDons().getById(id);
            if (hd == null)
            {
                return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (new LibData.Provider.NDHoaDons().Delete(id))
                {
                    return Json(new { status = 200, msg = "Xóa gọi món thành công.", data = new { TongTien = string.Format("{0:0,0 VNĐ}", new LibData.Provider.NDHoaDons().getTongTienTamTinh(hd.IdHoaDon)) } }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Xóa gọi món thất bại." }, JsonRequestBehavior.AllowGet);
                }

            }
        }
    }
}