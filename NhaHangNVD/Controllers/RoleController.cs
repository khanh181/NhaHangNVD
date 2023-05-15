using NhaHangNVD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhaHangNVD.Controllers
{
    [CustomAuthenticationFilter]
    public class RoleController : Controller
    {
        public ActionResult DanhMucRole()
        {
            try
            {
                var provider = new LibData.Provider.Roles();
                var list = provider.getAll();
                if(list == null)
                {
                    list = new List<LibData.Role>();
                }    
                return View(list);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public ActionResult ThemRole()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ThemRole(LibData.Role model)
        {
            var provider = new LibData.Provider.Roles();
            try
            {
                if (provider.checkexistRolename(model.Rolename, -1))
                {
                    return Json(new { status = 400, msg = "Tên quyền đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (provider.checkexistRolecode(model.Rolecode, -1))
                {
                    return Json(new { status = 400, msg = "Mã quyền đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }

                var Role = new LibData.Role()
                {
                    Rolename = model.Rolename,
                    Rolecode = model.Rolecode,
                    NgayTao = DateTime.Now
                };

                if (provider.Insert(Role))
                {

                    return Json(new { status = 200, msg = "Thêm quyền thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Thêm quyền thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SuaRole(int id)
        {
            try
            {
                var provider = new LibData.Provider.Roles();
                var modal = provider.getById(id);
                if (modal != null)
                {
                    return View(modal);
                }
                else
                {
                    return Json(new { status = 400, msg = "Không tìm thấy quyền." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //[CustomAuthorize("ADMIN")]
        [HttpPost]
        public ActionResult SuaRole(LibData.Role model)
        {
            var provider = new LibData.Provider.Roles();
            try
            {
                if (provider.checkexistRolename(model.Rolename, model.Id))
                {
                    return Json(new { status = 400, msg = "Tên quyền đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                if (provider.checkexistRolecode(model.Rolecode, model.Id))
                {
                    return Json(new { status = 400, msg = "Mã quyền đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                var Role = provider.getById(model.Id);

                Role.Rolename = model.Rolename;
                Role.NgaySua = DateTime.Now;
                if (provider.Update(Role))
                {
                    return Json(new { status = 200, msg = "Cập nhật quyền thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = 400, msg = "Cập nhật quyền thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[CustomAuthorize("ADMIN")]
        public JsonResult DeleteOneRole(int id)
        {
            var provider = new LibData.Provider.Roles();
            try
            {
                var model = provider.getById(id);
                if (model == null)
                {
                    return Json(new { status = 400, msg = "Quyền đã bị xóa hoặc không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
               
                else
                {
                    if (provider.Delete(id))
                    {
                        return Json(new { status = 200, msg = "Xóa quyền thành công, vui lòng tải lại trang!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 400, msg = "Xóa quyền thất bại!" }, JsonRequestBehavior.AllowGet);
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