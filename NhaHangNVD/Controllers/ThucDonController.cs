using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class ThucDonController : Controller
    {
        public ActionResult DanhMucThucDon()
        {
            try
            {
                var provider = new LibData.Provider.ThucDons();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.ThucDon>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemThucDon()
        {
            return View();
        }
        public ActionResult SuaThucDon(int id)
        {
            try
            {
                var provider = new LibData.Provider.ThucDons();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy thực đơn." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemThucDon(LibData.ThucDon model)
        {
            var provider = new LibData.Provider.ThucDons();
            try
            {
                if (provider.checkexistThucDon(model.TenThucDon, -1))
                {
                    return Json(new { status = 400, msg = "thực đơn đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var item = new LibData.ThucDon()
                {

                    TenThucDon = model.TenThucDon,
                    GioiThieu = model.GioiThieu,
                    NgayTao = DateTime.Now,
                };

                if (provider.Insert(item))
                {

                    return Json(new { status = 200, msg = "Thêm thực đơn thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm thực đơn thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        public ActionResult SuaThucDon(LibData.ThucDon model)
        {
            var provider = new LibData.Provider.ThucDons();
            try
            {
                if (provider.checkexistThucDon(model.TenThucDon, model.Id))
                {
                    return Json(new { status = 400, msg = "thực đơn đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                var ThucDon = provider.getById(model.Id);

                ThucDon.TenThucDon = model.TenThucDon;
                ThucDon.GioiThieu = model.GioiThieu;
                ThucDon.NgaySua = DateTime.Now;
                if (provider.Update(ThucDon))
                {
                    return Json(new { status = 200, msg = "Cập nhật thực đơn thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật thực đơn thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneThucDon(int id)
        {
            var provider = new LibData.Provider.ThucDons();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "thực đơn đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (model.MonAn.Count() > 0)
                {
                    return Json(new { status = 400, msg = "Vui lòng xóa các dữ liệu liên quan, trước khi xóa thực đơn!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa thực đơn thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa thực đơn thất bại!" }, JsonRequestBehavior.AllowGet);
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