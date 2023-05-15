using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class NhaCungCapController : Controller
    {
        public ActionResult DanhMucNhaCungCap()
        {
            try
            {
                var provider = new LibData.Provider.NCCs();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.NCC>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemNhaCungCap()
        {
            return View();
        }
        public ActionResult SuaNhaCungCap(int id)
        {
            try
            {
                var provider = new LibData.Provider.NCCs();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy nhà cung cấp." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemNhaCungCap(LibData.NCC model)
        {
            var provider = new LibData.Provider.NCCs();
            try
            {

                if (provider.checkexistSDT(model.SDT, -1))
                {
                    return Json(new { status = 400, msg = "SĐT đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var item = new LibData.NCC()
                {
                    TenToChuc = model.TenToChuc,
                    DiaChi = model.DiaChi,
                    SDT = model.SDT,
                    NgayTao = DateTime.Now,
                };
                var newId = provider.InsertButReturnId(item);
                if (newId != -1)
                { 
                    return Json(new { status = 200, msg = "Thêm nhà cung cấp thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm nhà cung cấp thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        public ActionResult SuaNhaCungCap(LibData.NCC model, int[] IdMatHang)
        {
            var provider = new LibData.Provider.NCCs();
            try
            {

                if (provider.checkexistSDT(model.SDT, model.Id))
                {
                    return Json(new { status = 400, msg = "SĐT đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var NhaCungCap = provider.getById(model.Id);

                NhaCungCap.TenToChuc = model.TenToChuc;
                NhaCungCap.DiaChi = model.DiaChi;
                NhaCungCap.SDT = model.SDT;
                NhaCungCap.NgaySua = DateTime.Now;
                if (provider.Update(NhaCungCap))
                {
                    return Json(new { status = 200, msg = "Cập nhật nhà cung cấp thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật nhà cung cấp thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneNhaCungCap(int id)
        {
            var provider = new LibData.Provider.NCCs();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "Nhà cung cấp đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa nhà cung cấp thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa nhà cung cấp thất bại!" }, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}