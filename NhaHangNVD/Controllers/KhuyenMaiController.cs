using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class KhuyenMaiController : Controller
    {
        public ActionResult DanhMucKhuyenMai()
        {
            try
            {
                var provider = new LibData.Provider.KhuyenMais();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.KhuyenMai>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemKhuyenMai()
        {
            return View();
        }
        public ActionResult SuaKhuyenMai(int id)
        {
            try
            {
                var provider = new LibData.Provider.KhuyenMais();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy khuyến mãi." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemKhuyenMai(LibData.KhuyenMai model)
        {
            var provider = new LibData.Provider.KhuyenMais();
            try
            {

                if (provider.checkexistSDT(model.Code, -1))
                {
                    return Json(new { status = 400, msg = "khuyến mãi đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var item = new LibData.KhuyenMai()
                {
                    Code = model.Code,
                    NoiDung = model.NoiDung,
                    KMPhanTram = model.KMPhanTram,
                    KMTienMat = model.KMTienMat,
                    TinhTrang = 1,
                    NgayTao = DateTime.Now,
                };
                var newId = provider.InsertButReturnId(item);
                if (newId != -1)
                {
                    return Json(new { status = 200, msg = "Thêm khuyến mãi thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm khuyến mãi thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        public ActionResult SuaKhuyenMai(LibData.KhuyenMai model)
        {
            var provider = new LibData.Provider.KhuyenMais();
            try
            {

                if (provider.checkexistSDT(model.Code, model.Id))
                {
                    return Json(new { status = 400, msg = "khuyến mãi đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var KhuyenMai = provider.getById(model.Id);
                if(KhuyenMai.TinhTrang == 0)
                {
                    return Json(new { status = 400, msg = "Mã khuyến mãi đã sử dụng không thể sửa!" }, JsonRequestBehavior.AllowGet);
                }    

                KhuyenMai.Code = model.Code;
                KhuyenMai.NoiDung = model.NoiDung;
                KhuyenMai.KMPhanTram = model.KMPhanTram;
                KhuyenMai.KMTienMat = model.KMTienMat;
                KhuyenMai.NgaySua = DateTime.Now;
                if (provider.Update(KhuyenMai))
                {
                    return Json(new { status = 200, msg = "Cập nhật khuyến mãi thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật khuyến mãi thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneKhuyenMai(int id)
        {
            var provider = new LibData.Provider.KhuyenMais();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "khuyến mãi đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                //if (model.NDPhieuNhap.Count() > 0)
                //{
                //    return Json(new { status = 400, msg = "khuyến mãi này đang được sử dụng trong phiếu nhập, không thể xóa!" }, JsonRequestBehavior.AllowGet);
                //}
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa khuyến mãi thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa khuyến mãi thất bại!" }, JsonRequestBehavior.AllowGet);
                    }
                }


            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult CheckCodeKMValid(string code)
        {
            var provider = new LibData.Provider.KhuyenMais();
            try
            {
                var model = provider.getByCode(code);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "khuyến mãi đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (model.TinhTrang == 0)
                {
                    return Json(new { status = 400, msg = "khuyến mãi đã được sử dụng!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 200 }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}