using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class BanAnController : Controller
    {
        public ActionResult DanhMucBanAn()
        {
            try
            {
                var provider = new LibData.Provider.BanAns();
                var list = provider.getAll();
                if (list == null)
                {
                    list = new List<LibData.BanAn>();
                }
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ThemBanAn()
        {
            return View();
        }
        public ActionResult SuaBanAn(int id)
        {
            try
            {
                var provider = new LibData.Provider.BanAns();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy bàn ăn." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemBanAn(LibData.BanAn model)
        {
            var provider = new LibData.Provider.BanAns();
            try
            {

                if (provider.checkexistSDT(model.TenBan, -1))
                {
                    return Json(new { status = 400, msg = "Bàn ăn đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var item = new LibData.BanAn()
                {
                    TenBan = model.TenBan,
                    TinhTrang = model.TinhTrang,
                    NgayTao = DateTime.Now,
                };
                var newId = provider.InsertButReturnId(item);
                if (newId != -1)
                {
                    return Json(new { status = 200, msg = "Thêm bàn ăn thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm bàn ăn thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        public ActionResult SuaBanAn(LibData.BanAn model, int[] IdMatHang)
        {
            var provider = new LibData.Provider.BanAns();
            try
            {

                if (provider.checkexistSDT(model.TenBan, model.Id))
                {
                    return Json(new { status = 400, msg = "Bàn ăn đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var BanAn = provider.getById(model.Id);

                BanAn.TenBan = model.TenBan;
                BanAn.TinhTrang = model.TinhTrang;
                BanAn.NgaySua = DateTime.Now;
                if (provider.Update(BanAn))
                {
                    return Json(new { status = 200, msg = "Cập nhật bàn ăn thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật bàn ăn thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneBanAn(int id)
        {
            var provider = new LibData.Provider.BanAns();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "bàn ăn đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                //if (model.NDPhieuNhap.Count() > 0)
                //{
                //    return Json(new { status = 400, msg = "bàn ăn này đang được sử dụng trong phiếu nhập, không thể xóa!" }, JsonRequestBehavior.AllowGet);
                //}
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa bàn ăn thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa bàn ăn thất bại!" }, JsonRequestBehavior.AllowGet);
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