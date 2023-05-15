using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class NhanVienController : Controller
    {
    public ActionResult DanhMucNhanVien()
    {
        try
        {
            var provider = new LibData.Provider.NhanViens();
            var list = provider.getAll();
            if (list == null)
            {
                list = new List<LibData.NhanVien>();
            }
            return View(list);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public ActionResult ThemNhanVien()
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
 
    public ActionResult SuaNhanVien(int id)
    {
        try
        {
            var provider = new LibData.Provider.NhanViens();
            var modal = provider.getById(id);
            if (modal != null)
            {
                return View(modal);
            }
            else
            {
                return Json(new { status = 400, msg = "Không tìm thấy nhân viên." }, JsonRequestBehavior.AllowGet);
            }

        }
        catch (Exception e)
        {

            return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
        }
    }

    [HttpPost]
    public JsonResult ThemNhanVien(LibData.NhanVien model, string rePassword)
    {
        var provider = new LibData.Provider.NhanViens();
        try
        {
            if (provider.checkexistTaiKhoan(model.TenNhanVien, -1))
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

            var item = new LibData.NhanVien()
            {
                TenNhanVien = model.TenNhanVien,
                Email = model.Email,
                Password = model.Password,
                DienThoai = model.DienThoai,
                NgaySinh = model.NgaySinh,
                QueQuan = model.QueQuan,
                NgayTao = DateTime.Now,
            };
            var newId = provider.InsertButReturnId(item);
            if (newId != -1)
            {
                return Json(new { status = 200, msg = "Thêm nhân viên thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = 400, msg = "Thêm nhân viên thất bại!" }, JsonRequestBehavior.AllowGet);
            }

        }
        catch (Exception e)
        {
            return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
        }
    }
    //[CustomAuthorize("ADMIN")]
    [HttpPost]
    public ActionResult SuaNhanVien(LibData.NhanVien model, string rePassword)
    {
        var provider = new LibData.Provider.NhanViens();
        try
        {
            var NhanVien = provider.getById(model.Id);

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
                NhanVien.Password = model.Password;
            }

            NhanVien.TenNhanVien = model.TenNhanVien;
            NhanVien.DienThoai = model.DienThoai;
            NhanVien.NgaySinh = model.NgaySinh;
            NhanVien.QueQuan = model.QueQuan;
            NhanVien.NgayTao = DateTime.Now;
            if (provider.Update(NhanVien))
            {
                return Json(new { status = 200, msg = "Cập nhật nhân viên thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = 400, msg = "Cập nhật nhân viên thất bại!" }, JsonRequestBehavior.AllowGet);
            }

        }
        catch (Exception e)
        {
            return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
        }
    }

    [HttpPost]
    //[CustomAuthorize("ADMIN")]
    public JsonResult DeleteOneNhanVien(int id)
    {
        var provider = new LibData.Provider.NhanViens();
        try
        {
            var model = provider.getById(id);
            if (model == null)
            {
                return Json(new { status = 400, msg = "Nhân viên đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (provider.Delete(id))
                {
                    return Json(new { status = 200, msg = "Xóa nhân viên thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Xóa nhân viên thất bại!" }, JsonRequestBehavior.AllowGet);
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