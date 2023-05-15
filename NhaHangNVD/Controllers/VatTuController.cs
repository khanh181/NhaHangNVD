using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class VatTuController : Controller
    {
        public ActionResult DanhMucVatTu()
        {
            try
            {
                var provider = new LibData.Provider.VatTus();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.VatTu>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemVatTu()
        {
            return View();
        }
        public ActionResult SuaVatTu(int id)
        {
            try
            {
                var provider = new LibData.Provider.VatTus();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy vật tư." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemVatTu(LibData.VatTu model)
        {
            var provider = new LibData.Provider.VatTus();
            try
            {
                if (provider.checkexistVatTu(model.TenVT, -1))
                {
                    return Json(new { status = 400, msg = "vật tư đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var item = new LibData.VatTu()
                {

                    TenVT = model.TenVT,
                    SoLuong = model.SoLuong,
                    DonVi = model.DonVi,
                    NgayTao = DateTime.Now,
                };

                if (provider.Insert(item))
                {

                    return Json(new { status = 200, msg = "Thêm vật tư thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm vật tư thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        public ActionResult SuaVatTu(LibData.VatTu model)
        {
            var provider = new LibData.Provider.VatTus();
            try
            {
                if (provider.checkexistVatTu(model.TenVT, model.Id))
                {
                    return Json(new { status = 400, msg = "vật tư đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                var VatTu = provider.getById(model.Id);

                VatTu.TenVT = model.TenVT;
                VatTu.SoLuong = model.SoLuong;
                VatTu.DonVi =  model.DonVi;
                VatTu.NgaySua = DateTime.Now;
                if (provider.Update(VatTu))
                {
                    return Json(new { status = 200, msg = "Cập nhật vật tư thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật vật tư thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneVatTu(int id)
        {
            var provider = new LibData.Provider.VatTus();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "vật tư đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                //if (model.PhatHang.Count() > 0)
                //{
                //    return Json(new { status = 400, msg = "Vui lòng xóa các dữ liệu liên quan, trước khi xóa vật tư!" }, JsonRequestBehavior.AllowGet);
                //}
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa vật tư thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa vật tư thất bại!" }, JsonRequestBehavior.AllowGet);
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