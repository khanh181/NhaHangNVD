using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class MonAnController : Controller
    {
        public ActionResult DanhMucMonAn()
        {
            try
            {
                var provider = new LibData.Provider.MonAns();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.MonAn>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemMonAn()
        {
            ViewData["listThucDon"] = new LibData.Provider.ThucDons().getAll();
            ViewData["listVatTu"] = new LibData.Provider.VatTus().getAll();
            return View();
        }
        public ActionResult SuaMonAn(int id)
        {
            try
            {
                var provider = new LibData.Provider.MonAns();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    ViewData["listThucDon"] = new LibData.Provider.ThucDons().getAll();
                    ViewData["listVatTu"] = new LibData.Provider.VatTus().getAll();
                    ViewData["listCTMA"] = new LibData.Provider.CongThucMonAns().getAllByIdMonAn(id);
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy món ăn." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ThemMonAn(LibData.MonAn model, HttpPostedFileBase Avatar, List<LibData.CongThucMonAn> ctma)
        {
            var provider = new LibData.Provider.MonAns();
            try
            {
                if (provider.checkexistMonAn(model.TenMon, -1))
                {
                    return Json(new { status = 400, msg = "món ăn đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                var fileNameIcon = UpLoadFileLocal(Avatar);
                if (string.IsNullOrEmpty(fileNameIcon))
                {
                    return Json(new { status = 400, msg = "Upload ảnh icon thất bại!" }, JsonRequestBehavior.AllowGet);
                }
                var item = new LibData.MonAn()
                {

                    TenMon = model.TenMon,
                    IdThucDon = model.IdThucDon,
                    GioiThieu = model.GioiThieu,
                    GiaTien = model.GiaTien,
                    GioiThieuNgan = model.GioiThieuNgan,
                    IsDacBiet = model.IsDacBiet,
                    HinhAnh = fileNameIcon,
                    NgayTao = DateTime.Now,
                };
                var newId = provider.InsertButReturnId(item);
                if (newId != -1)
                {
                    var provider_ctma = new LibData.Provider.CongThucMonAns();
                    foreach (var ct in ctma)
                    {
                        var temp = new LibData.CongThucMonAn()
                        {
                            IdMonAn = newId,
                            IdVatTu = ct.IdVatTu,
                            SoLuong = ct.SoLuong,
                            NgayTao = DateTime.Now,
                        };
                        provider_ctma.Insert(temp);
                    }
                    return Json(new { status = 200, msg = "Thêm món ăn thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FileInfo fileIcon = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + fileNameIcon);
                    if (fileIcon.Exists)
                    {
                        fileIcon.Delete();
                    }
                    return Json(new { status = 400, msg = "Thêm món ăn thất bại!" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult SuaMonAn(LibData.MonAn model, HttpPostedFileBase Avatar)
        {
            var provider = new LibData.Provider.MonAns();
            try
            {
                if (provider.checkexistMonAn(model.TenMon, model.Id))
                {
                    return Json(new { status = 400, msg = "món ăn đã tồn tại!" }, JsonRequestBehavior.AllowGet);
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
                
                var MonAn = provider.getById(model.Id);

                MonAn.TenMon = model.TenMon;
                MonAn.IdThucDon = model.IdThucDon;
                MonAn.GioiThieu = model.GioiThieu;
                MonAn.GiaTien = model.GiaTien;
                MonAn.IsDacBiet = model.IsDacBiet;
                MonAn.GioiThieuNgan = model.GioiThieuNgan;
                if(fileNameIcon != string.Empty) MonAn.HinhAnh = fileNameIcon;
                MonAn.NgaySua = DateTime.Now;
                if (provider.Update(MonAn))
                {
                    return Json(new { status = 200, msg = "Cập nhật món ăn thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FileInfo fileIcon = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + fileNameIcon);
                    if (fileIcon.Exists)
                    {
                        fileIcon.Delete();
                    }
                    return Json(new { status = 400, msg = "Cập nhật món ăn thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneMonAn(int id)
        {
            var provider = new LibData.Provider.MonAns();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "món ăn đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                //if (model.ThucDon.Count() > 0)
                //{
                //    return Json(new { status = 400, msg = "Vui lòng xóa các dữ liệu liên quan, trước khi xóa món ăn!" }, JsonRequestBehavior.AllowGet);
                //}
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa món ăn thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa món ăn thất bại!" }, JsonRequestBehavior.AllowGet);
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
                        basePath = "/Upload/Img/MonAn/";
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

        [HttpPost]
        public JsonResult ThemCongThucMonAn(LibData.CongThucMonAn model)
        {
            var provider = new LibData.Provider.CongThucMonAns();

            try
            {
                if (provider.checkexistCTMA(model.IdVatTu, model.IdMonAn, -1))
                {
                    return Json(new { status = 400, msg = "Vật liệu đã tồn tại trong công thức!" }, JsonRequestBehavior.AllowGet);
                }
                var ct = new LibData.CongThucMonAn()
                {
                    IdMonAn = model.IdMonAn,
                    IdVatTu = model.IdVatTu,
                    SoLuong = model.SoLuong,
                    NgayTao = DateTime.Now,
                };
                var newId = provider.InsertButReturnId(ct);
                if (newId != -1)
                {
                    return Json(new { status = 200, msg = newId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm thất bại!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SuaCongThucMonAn(LibData.CongThucMonAn model)
        {
            var provider = new LibData.Provider.CongThucMonAns();

            try
            {
                var value = provider.getById(model.Id);
                if (provider.checkexistCTMA(model.IdVatTu, model.IdMonAn, model.Id))
                {
                    return Json(new { status = 400, msg = "Vật liệu đã tồn tại trong công thức!" }, JsonRequestBehavior.AllowGet);
                }
                value.IdVatTu = model.IdVatTu;
                value.SoLuong = model.SoLuong;
                value.NgaySua = DateTime.Today;
                if (provider.Update(value))
                {
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

        [HttpPost]
        public JsonResult DeleteCTMA(int id)
        {
            var provider = new LibData.Provider.CongThucMonAns();
            var model = provider.getById(id);
            if (model != null)
            {
                try
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa thất bại!" }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception e)
                {
                    return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { status = 400, msg = "Chi tiết không tồn tại!" }, JsonRequestBehavior.AllowGet);
        }
    }
}