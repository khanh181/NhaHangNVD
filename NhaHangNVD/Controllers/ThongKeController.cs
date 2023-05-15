using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class ThongKeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                ViewData["listBanAn"] = new LibData.Provider.BanAns().getAll();
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ListHoaDon(string datetimerange, string type, int? idBan, string searchKeyword)
        {
            try
            {
                var datetime = datetimerange.Split('-');
                DateTime from, to;
                DateTime.TryParse(datetime[0], out from);
                DateTime.TryParse(datetime[1], out to);
                var model = new LibData.Provider.ThongKes().getAllHoaDonByThongKe(from, to, type, idBan, searchKeyword);
                if(model == null)
                {
                    model = new List<LibData.HoaDon>();
                }
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}