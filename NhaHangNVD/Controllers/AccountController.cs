using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            var provider = new LibData.Provider.NhanViens();
            var user = provider.getByEmail(Username);
            if (user != null)
            {
                /*if (user.UserRoles.Count(x => x.Role.Rolecode.Equals("ADMIN") || x.Role.Rolecode.Equals("REPORTER")) == 0)
                {
                    ModelState.AddModelError("error", "Tài khoản không hợp lệ.");
                    return View();
                }*/
                if (Password.Equals(user.Password))
                {
                    Session["FullName"] = user.TenNhanVien;
                    Session["UserName"] = user.Email;
                    Session["UserId"] = user.Id;
                    Session["UserRole"] = new List<string>() { "ADMIN" }; //user.NhanVienRole.Select(x => x.Role.Rolecode).ToList();

                    return Redirect("/Home/IndexAdmin");

                }
                else
                {
                    ModelState.AddModelError("error", "Mật khẩu không chính xác vui lòng thử lại");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("error", "Tên tài khoản không tồn tại vui lòng thử lại.");
                return View();
            }
        }

        [CustomAuthenticationFilter]
        public ActionResult InfoUser()
        {
            var user = new LibData.Provider.NhanViens().getByEmail(Session["UserName"].ToString());

            return View(user);
        }

        [CustomAuthenticationFilter]
        [HttpPost]
        public ActionResult InfoUser(LibData.NhanVien model)
        {
            var provider = new LibData.Provider.NhanViens();
            try
            {
                var NhanVien = provider.getById(model.Id);

                if (provider.checkexistSDT(model.DienThoai, model.Id))
                {
                    return Json(new { status = 400, msg = "SĐT đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                NhanVien.TenNhanVien = model.TenNhanVien;
                NhanVien.QueQuan = model.QueQuan;
                NhanVien.DienThoai = model.DienThoai;
                NhanVien.NgaySua = DateTime.Now;
                if (provider.Update(NhanVien))
                {
                    Session["FullName"] = model.TenNhanVien;
                    return Json(new { status = 200, msg = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthenticationFilter]
        public ActionResult ChangePassword()
        {
            var user = new LibData.Provider.NhanViens().getByEmail(Session["UserName"].ToString());

            return View(user);
        }

        [CustomAuthenticationFilter]
        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string reNewPassword, int Id)
        {
            var provider = new LibData.Provider.NhanViens();
            try
            {
                var NhanVien = provider.getById(Id);
                if (!OldPassword.Equals(NhanVien.Password))
                {
                    return Json(new { status = 400, msg = "Mật khẩu hiện tại không đúng!" }, JsonRequestBehavior.AllowGet);
                }
                if (!NewPassword.Equals(reNewPassword))
                {
                    return Json(new { status = 400, msg = "Mật khẩu mới và nhập lại không khớp!" }, JsonRequestBehavior.AllowGet);
                }
                NhanVien.Password = NewPassword;
                if (provider.Update(NhanVien))
                {
                    Session["FullName"] = null;
                    Session["UserName"] = null;
                    Session["UserId"] = null;
                    Session["UserRole"] = null;

                    return Json(new { status = 200, msg = "Thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthenticationFilter]
        public ActionResult Logout()
        {
            Session["FullName"] = null;
            Session["UserName"] = null;
            Session["UserId"] = null;
            Session["UserRole"] = null;

            return Redirect("/Account/Login");
        }
    }
}