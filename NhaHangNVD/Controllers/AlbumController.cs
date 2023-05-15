using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class AlbumController : Controller
    {
        public ActionResult DanhMucAlbum()
        {
            try
            {
                var provider = new LibData.Provider.Albums();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.Album>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemAlbum()
        {
            //ViewData["listThucDon"] = new LibData.Provider.ThucDons().getAll();
            return View();
        }
        public ActionResult SuaAlbum(int id)
        {
            try
            {
                var provider = new LibData.Provider.Albums();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    //ViewData["listThucDon"] = new LibData.Provider.ThucDons().getAll();
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy ảnh." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ThemAlbum(LibData.Album model, HttpPostedFileBase Avatar)
        {
            var provider = new LibData.Provider.Albums();
            try
            {
                if (provider.checkexistAlbum(model.TenAnh, -1))
                {
                    return Json(new { status = 400, msg = "ảnh đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                var fileNameIcon = UpLoadFileLocal(Avatar);
                if (string.IsNullOrEmpty(fileNameIcon))
                {
                    return Json(new { status = 400, msg = "Upload ảnh icon thất bại!" }, JsonRequestBehavior.AllowGet);
                }
                var item = new LibData.Album()
                {

                    LoaiAnh = model.LoaiAnh,
                    TenAnh = model.TenAnh,
                    GhiChu = model.GhiChu,
                    HinhAnh = fileNameIcon,
                    NgayTao = DateTime.Now,
                };

                if (provider.Insert(item))
                {

                    return Json(new { status = 200, msg = "Thêm ảnh thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FileInfo fileIcon = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + fileNameIcon);
                    if (fileIcon.Exists)
                    {
                        fileIcon.Delete();
                    }
                    return Json(new { status = 400, msg = "Thêm ảnh thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SuaAlbum(LibData.Album model, HttpPostedFileBase Avatar)
        {
            var provider = new LibData.Provider.Albums();
            try
            {
                if (provider.checkexistAlbum(model.TenAnh, model.Id))
                {
                    return Json(new { status = 400, msg = "ảnh đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var fileNameIcon = string.Empty;
                if (Avatar != null)
                {
                    fileNameIcon = UpLoadFileLocal(Avatar);
                    if (string.IsNullOrEmpty(fileNameIcon))
                    {
                        return Json(new { status = 400, msg = "Upload ảnh icon thất bại!" }, JsonRequestBehavior.AllowGet);
                    }
                }    
                
                var Album = provider.getById(model.Id);

                Album.LoaiAnh = model.LoaiAnh;
                Album.TenAnh = model.TenAnh;
                Album.GhiChu = model.GhiChu;
                if (fileNameIcon != string.Empty) Album.HinhAnh = fileNameIcon;
                Album.NgaySua = DateTime.Now;
                if (provider.Update(Album))
                {
                    return Json(new { status = 200, msg = "Cập nhật ảnh thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FileInfo fileIcon = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + fileNameIcon);
                    if (fileIcon.Exists)
                    {
                        fileIcon.Delete();
                    }
                    return Json(new { status = 400, msg = "Cập nhật ảnh thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneAlbum(int id)
        {
            var provider = new LibData.Provider.Albums();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "ảnh đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                //if (model.ThucDon.Count() > 0)
                //{
                //    return Json(new { status = 400, msg = "Vui lòng xóa các dữ liệu liên quan, trước khi xóa album!" }, JsonRequestBehavior.AllowGet);
                //}
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa ảnh thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa ảnh thất bại!" }, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // Xử lý file ảnh
        public string UpLoadFileLocal(HttpPostedFileBase file)
        {
            var message = "";
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName).Replace(" ", "-");
                    string basePath = "";
                    if (_FileName.Contains(".jpg") || _FileName.Contains(".png") || _FileName.Contains(".svg"))
                    {
                        basePath = "/Upload/Img/Album/";
                        string path = AppDomain.CurrentDomain.BaseDirectory + basePath;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        _FileName = DateTime.Now.Ticks.ToString() + "-" + _FileName;
                        file.SaveAs(path + _FileName);
                        //string protocol = HttpContext.Request.Url.Scheme;
                        //string domain = HttpContext.Request.Url.Host.ToLower();
                        // message = protocol + "://" + domain + basePath.Replace("~/", "") + _FileName;
                        message = basePath.Replace("~", "") + _FileName;
                    }
                    else
                    {
                        message = "";
                    }

                }
            }
            catch (Exception e)
            {
                message = "";
            }
            return message;
        }
    }
}