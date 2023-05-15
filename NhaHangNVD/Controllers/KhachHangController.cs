using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class KhachHangController : Controller
    {
        public ActionResult DanhMucKhachHang()
        {
            try
            {
                var provider = new LibData.Provider.KhachHangs();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.KhachHang>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ThemKhachHang()
        {
            try
            {

                return View();


            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SuaKhachHang(int id)
        {
            try
            {
                var provider = new LibData.Provider.KhachHangs();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy khách hàng." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemKhachHang(LibData.KhachHang model, string rePassword)
        {
            var provider = new LibData.Provider.KhachHangs();
            try
            {
                if (provider.checkexistTaiKhoan(model.Email, -1))
                {
                    return Json(new { status = 400, msg = "Tài khoản đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (provider.checkexistSDT(model.DienThoai, -1))
                {
                    return Json(new { status = 400, msg = "SĐT đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (provider.checkexistEmail(model.Email, -1))
                {
                    return Json(new { status = 400, msg = "Email đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (model.Password.Equals(rePassword) == false)
                {
                    return Json(new { status = 400, msg = "Mật khẩu nhập lại không khớp!" }, JsonRequestBehavior.AllowGet);
                }

                var item = new LibData.KhachHang()
                {
                    TenKhach = model.TenKhach,
                    Email = model.Email,
                    Password = model.Password,
                    DienThoai = model.DienThoai,
                    NgayTao = DateTime.Now,
                };
                var newId = provider.InsertButReturnId(item);
                if (newId != -1)
                {
                    return Json(new { status = 200, msg = "Thêm khách hàng thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm khách hàng thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        public ActionResult SuaKhachHang(LibData.KhachHang model, string rePassword)
        {
            var provider = new LibData.Provider.KhachHangs();
            try
            {
                var KhachHang = provider.getById(model.Id);

                if (provider.checkexistSDT(model.DienThoai, model.Id))
                {
                    return Json(new { status = 400, msg = "SĐT đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (provider.checkexistEmail(model.Email, model.Id))
                {
                    return Json(new { status = 400, msg = "Email đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Password) == false && string.IsNullOrEmpty(rePassword) == false)
                {
                    if (model.Password.Equals(rePassword) == false)
                    {
                        return Json(new { status = 400, msg = "Mật khẩu nhập lại không khớp!" }, JsonRequestBehavior.AllowGet);
                    }
                    KhachHang.Password = model.Password;
                }

                KhachHang.TenKhach = model.TenKhach;
                KhachHang.DienThoai = model.DienThoai;
                KhachHang.NgayTao = DateTime.Now;
                if (provider.Update(KhachHang))
                {
                    return Json(new { status = 200, msg = "Cập nhật khách hàng thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật khách hàng thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneKhachHang(int id)
        {
            var provider = new LibData.Provider.KhachHangs();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "khách hàng đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa khách hàng thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa khách hàng thất bại!" }, JsonRequestBehavior.AllowGet);
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