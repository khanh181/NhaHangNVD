using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibData.Config;
using static LibData.Helper.StringHelper;

namespace NhaHangNVD.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        public ActionResult DanhMucNhanVien()
        {
            var provider = new LibData.Provider.NhanVien();
            var list = provider.getAll().Where(x => x.TinhTrang != ((int)CommonConfig.CommonStatus.Delete)).ToList();
            return View(list);
        }

        public ActionResult ThemNhanVien()
        {
            // khởi tạo với Id = 0 để biết là model rỗng, trangthai = hoạt động
            var model = new LibData.NhanVien()
            {
                Id = 0,
                TinhTrang = (int)CommonConfig.CommonStatus.Normal
            };

            // biến nhớ tạm để phía view biết đc cần gọi function addNhanVien()
            ViewBag.functionSubmit = "addNhanVien()";
            // biến nhớ tạm để phía view biết title
            ViewBag.Title = "Thêm nhân viên";
            return View("AddOrEditNhanVien", model);
        }

        [HttpPost]
        public JsonResult ThemNhanVien(LibData.NhanVien model)
        {
            try
            {
                var provider = new LibData.Provider.NhanVien();
                //loop trim string in item
                var stringProperties = model.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string));

                foreach (var stringProperty in stringProperties)
                {
                    string currentValue = (string)stringProperty.GetValue(model, null);
                    stringProperty.SetValue(model, TrimIfNotNull(currentValue), null);
                }
                model.NgayTao = DateTime.Now;
                if (provider.Insert(model))
                {
                    return Json(new { status = (int)CommonConfig.CommonResponse.Success, msg = CommonConfig.CommonResponse.Success.GetDescription() }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = (int)CommonConfig.CommonResponse.FailFromClient400, msg = CommonConfig.CommonResponse.FailFromClient400.GetDescription() }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = (int)CommonConfig.CommonResponse.FailFromServer, msg = CommonConfig.CommonResponse.FailFromServer.GetDescription() }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult SuaNhanVien(int id)
        {
            var provider = new LibData.Provider.NhanVien();
            var model = provider.getById(id);
            if(model != null)
            {
                // biến nhớ tạm để phía view biết đc cần gọi function editNhanVien()
                ViewBag.functionSubmit = "editNhanVien()";
                // biến nhớ tạm để phía view biết title
                ViewBag.Title = "Sửa nhân viên";
                return View("AddOrEditNhanVien", model);
            }
            else
            {
                return Json(new { status = (int)CommonConfig.CommonResponse.FailFromClient404, msg = CommonConfig.CommonResponse.FailFromClient404.GetDescription() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SuaNhanVien(LibData.NhanVien model)
        {
            try
            {
                var provider = new LibData.Provider.NhanVien();
                var NhanVien = provider.getById(model.Id);
                if(NhanVien != null)
                {
                    //loop trim string in item
                    var stringProperties = model.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(string));

                    foreach (var stringProperty in stringProperties)
                    {
                        string currentValue = (string)stringProperty.GetValue(model, null);
                        stringProperty.SetValue(model, TrimIfNotNull(currentValue), null);
                    }
                    NhanVien.TenNhanVien = model.TenNhanVien;
                    NhanVien.Email = model.Email;
                    NhanVien.Password = model.Password;
                    NhanVien.DienThoai = model.DienThoai;
                    NhanVien.NgaySinh = model.NgaySinh;
                    NhanVien.QueQuan = model.QueQuan;
                    NhanVien.TinhTrang = model.TinhTrang;
                    NhanVien.NgaySua = DateTime.Now;
                    if (provider.Update(NhanVien))
                    {
                        return Json(new { status = (int)CommonConfig.CommonResponse.Success, msg = CommonConfig.CommonResponse.Success.GetDescription() }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = (int)CommonConfig.CommonResponse.FailFromClient400, msg = CommonConfig.CommonResponse.FailFromClient400.GetDescription() }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = (int)CommonConfig.CommonResponse.FailFromClient404, msg = CommonConfig.CommonResponse.FailFromClient404.GetDescription() }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = (int)CommonConfig.CommonResponse.FailFromServer, msg = CommonConfig.CommonResponse.FailFromServer.GetDescription() }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult XoaNhanVien(int id)
        {
            try
            {
                var provider = new LibData.Provider.NhanVien();
                var model = provider.getById(id);
                if (model != null)
                {
                    if(model.PhieuNhap.Count() > 0)
                    {
                        return Json(new { status = (int)CommonConfig.CommonResponse.FailFromClient405, msg = CommonConfig.CommonResponse.FailFromClient405.GetDescription() }, JsonRequestBehavior.AllowGet);
                    }
                    if (provider.Delete(id))
                    {
                        return Json(new { status = (int)CommonConfig.CommonResponse.Success, msg = CommonConfig.CommonResponse.Success.GetDescription() }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = (int)CommonConfig.CommonResponse.FailFromClient400, msg = CommonConfig.CommonResponse.FailFromClient400.GetDescription() }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = (int)CommonConfig.CommonResponse.FailFromClient404, msg = CommonConfig.CommonResponse.FailFromClient404.GetDescription() }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { status = (int)CommonConfig.CommonResponse.FailFromServer, msg = CommonConfig.CommonResponse.FailFromServer.GetDescription() }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}