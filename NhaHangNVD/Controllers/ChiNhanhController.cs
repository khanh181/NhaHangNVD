using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class ChiNhanhController : Controller
    {
        public ActionResult DanhMucChiNhanh()
        {
            try
            {
                var provider = new LibData.Provider.ChiNhanhs();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.ChiNhanh>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemChiNhanh()
        {
            return View();
        }
        public ActionResult SuaChiNhanh(int id)
        {
            try
            {
                var provider = new LibData.Provider.ChiNhanhs();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy chi nhánh." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemChiNhanh(LibData.ChiNhanh model)
        {
            var provider = new LibData.Provider.ChiNhanhs();
            try
            {
                if (provider.checkexistChiNhanh(model.Ten, -1))
                {
                    return Json(new { status = 400, msg = "chi nhánh đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var item = new LibData.ChiNhanh()
                {

                    Ten = model.Ten,
                    DiaChi = model.DiaChi,
                    NgayTao = DateTime.Now,
                    NgaySua = model.NgaySua,
                };

                if (provider.Insert(item))
                {

                    return Json(new { status = 200, msg = "Thêm chi nhánh thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm chi nhánh thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        public ActionResult SuaChiNhanh(LibData.ChiNhanh model)
        {
            var provider = new LibData.Provider.ChiNhanhs();
            try
            {
                if (provider.checkexistChiNhanh(model.Ten, model.Id))
                {
                    return Json(new { status = 400, msg = "chi nhánh đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                var ChiNhanh = provider.getById(model.Id);

                ChiNhanh.Ten = model.Ten;
                ChiNhanh.DiaChi = model.DiaChi;
                ChiNhanh.NgaySua = DateTime.Now;
                if (provider.Update(ChiNhanh))
                {
                    return Json(new { status = 200, msg = "Cập nhật chi nhánh thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật chi nhánh thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneChiNhanh(int id)
        {
            var provider = new LibData.Provider.ChiNhanhs();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "chi nhánh đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                //if (model.PhatHang.Count() > 0)
                //{
                //    return Json(new { status = 400, msg = "Vui lòng xóa các dữ liệu liên quan, trước khi xóa chi nhánh!" }, JsonRequestBehavior.AllowGet);
                //}
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa chi nhánh thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa chi nhánh thất bại!" }, JsonRequestBehavior.AllowGet);
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