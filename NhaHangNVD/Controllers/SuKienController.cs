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
    public class SuKienController : Controller
    {
        public ActionResult DanhMucSuKien()
        {
            try
            {
                var provider = new LibData.Provider.SuKiens();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.SuKien>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemSuKien()
        {
            //ViewData["listThucDon"] = new LibData.Provider.ThucDons().getAll();
            return View();
        }
        public ActionResult SuaSuKien(int id)
        {
            try
            {
                var provider = new LibData.Provider.SuKiens();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    //ViewData["listThucDon"] = new LibData.Provider.ThucDons().getAll();
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy sự kiện." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ThemSuKien(LibData.SuKien model, HttpPostedFileBase Avatar)
        {
            var provider = new LibData.Provider.SuKiens();
            try
            {
                if (provider.checkexistSuKien(model.TieuDe, -1))
                {
                    return Json(new { status = 400, msg = "sự kiện đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                var fileNameIcon = UpLoadFileLocal(Avatar);
                if (string.IsNullOrEmpty(fileNameIcon))
                {
                    return Json(new { status = 400, msg = "Upload ảnh icon thất bại!" }, JsonRequestBehavior.AllowGet);
                }
                var item = new LibData.SuKien()
                {

                    TieuDe = model.TieuDe,
                    ChiPhi = model.ChiPhi,
                    MoTa = model.MoTa,
                    HinhAnh = fileNameIcon,
                    NgayTao = DateTime.Now,
                };

                if (provider.Insert(item))
                {

                    return Json(new { status = 200, msg = "Thêm sự kiện thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FileInfo fileIcon = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + fileNameIcon);
                    if (fileIcon.Exists)
                    {
                        fileIcon.Delete();
                    }
                    return Json(new { status = 400, msg = "Thêm sự kiện thất bại!" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult SuaSuKien(LibData.SuKien model, HttpPostedFileBase Avatar)
        {
            var provider = new LibData.Provider.SuKiens();
            try
            {
                if (provider.checkexistSuKien(model.TieuDe, model.Id))
                {
                    return Json(new { status = 400, msg = "sự kiện đã tồn tại!" }, JsonRequestBehavior.AllowGet);
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
                
                var SuKien = provider.getById(model.Id);

                SuKien.TieuDe = model.TieuDe;
                SuKien.ChiPhi = model.ChiPhi;
                SuKien.MoTa = model.MoTa;
                if(fileNameIcon != string.Empty) SuKien.HinhAnh = fileNameIcon;
                SuKien.NgaySua = DateTime.Now;
                if (provider.Update(SuKien))
                {
                    return Json(new { status = 200, msg = "Cập nhật sự kiện thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FileInfo fileIcon = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + fileNameIcon);
                    if (fileIcon.Exists)
                    {
                        fileIcon.Delete();
                    }
                    return Json(new { status = 400, msg = "Cập nhật sự kiện thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneSuKien(int id)
        {
            var provider = new LibData.Provider.SuKiens();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "sự kiện đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                //if (model.ThucDon.Count() > 0)
                //{
                //    return Json(new { status = 400, msg = "Vui lòng xóa các dữ liệu liên quan, trước khi xóa sự kiện!" }, JsonRequestBehavior.AllowGet);
                //}
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa sự kiện thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa sự kiện thất bại!" }, JsonRequestBehavior.AllowGet);
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
                        basePath = "/Upload/Img/SuKien/";
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