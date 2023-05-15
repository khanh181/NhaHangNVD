using LibData.Config;
using LibData.Models;
using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var T1 = new LibData.Provider.ThucDons().getAllAsync();
            var T2 = new LibData.Provider.MonAns().getSpecialAsync();
            var T3 = new LibData.Provider.SuKiens().getAllAsync();
            var T4 = new LibData.Provider.Albums().getAllAsync();
            var T5 = new LibData.Provider.Albums().getAllDauBepAsync();
            var T6 = new LibData.Provider.Albums().getAllAvaAsync();
            var T7 = new LibData.Provider.Albums().getAllCharAsync();
            await Task.WhenAll(T1, T2, T3, T4, T5, T6, T7);
            var model = new HomeViewData()
            {
                Menus = T1.Result,
                MonAnDacBiets = T2.Result,
                SuKiens = T3.Result,
                Albums = T4.Result,
                DauBeps = T5.Result,
                Avas = T6.Result,
                Chars = T7.Result,
            };
            return View(model);
        }

        [CustomAuthenticationFilter]
        public ActionResult IndexAdmin()
        {
            var model = new LibData.Provider.BanAns().getAll();
            if(model != null)
            {
                foreach (var item in model)
                {
                    if (item.TinhTrang != (int)CommonConfig.BanStatus.Hong) 
                    {
                        var listHd = new LibData.Provider.HoaDons().getAllHoaDonByIdBan(item.Id);
                        if (listHd == null || listHd.Count == 0)
                        {
                            item.TinhTrang = (int)CommonConfig.BanStatus.Trong;

                        }
                        else
                        {
                            if (DateTime.Now >= listHd[0].ThoiGianDen)
                            {
                                item.TinhTrang = (int)CommonConfig.BanStatus.Ban;
                            }
                            else if (DateTime.Now < listHd[0].ThoiGianDen)
                            {
                                item.TinhTrang = (int)CommonConfig.BanStatus.DuocDat;
                            }
                        }
                        new LibData.Provider.BanAns().Update(item);
                    }
                }
                model = new LibData.Provider.BanAns().getAll();
                return View(model);
            }
            return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthenticationFilter]
        public JsonResult ListBanTrangThai()
        {
            var model = new LibData.Provider.BanAns().getAll();
            if(model != null)
            {
                foreach (var item in model)
                {
                    if (item.TinhTrang != (int)CommonConfig.BanStatus.Hong)
                    {
                        var listHd = new LibData.Provider.HoaDons().getAllHoaDonByIdBan(item.Id);
                        if (listHd == null || listHd.Count == 0)
                        {
                            item.TinhTrang = (int)CommonConfig.BanStatus.Trong;

                        }
                        else
                        {
                            if (DateTime.Now >= listHd[0].ThoiGianDen)
                            {
                                item.TinhTrang = (int)CommonConfig.BanStatus.Ban;
                            }
                            else if (DateTime.Now < listHd[0].ThoiGianDen)
                            {
                                item.TinhTrang = (int)CommonConfig.BanStatus.DuocDat;
                            }
                        }
                        new LibData.Provider.BanAns().Update(item);
                    }
                }
                model = new LibData.Provider.BanAns().getAll();
                var data = model.Select(x => new { x.Id, x.TinhTrang, MauSac = string.Format("https://via.placeholder.com/300/{0}?text={1}", LibData.Config.CommonConfig.BanStatusToDictionary[x.TinhTrang], x.TenBan) });
                return Json(new { status = 200, data = data }, JsonRequestBehavior.AllowGet);
            }    
            return Json(new { status = 400, msg = "Không tìm thấy dữ liệu." }, JsonRequestBehavior.AllowGet);
        }
    }
}