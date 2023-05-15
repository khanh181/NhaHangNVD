using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class KhachHangs : ApplicationDbContexts
    {

        public KhachHang getByEmail(string username)
        {
            try
            {
                var userInfo = ApplicationDbContext.KhachHang.Where(x => x.Email.Equals(username)).FirstOrDefault();
                if (userInfo != null)
                {
                    return userInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public KhachHang getBySDT(string sdt)
        {
            try
            {
                var userInfo = ApplicationDbContext.KhachHang.Where(x => x.DienThoai.Equals(sdt)).FirstOrDefault();
                if (userInfo != null)
                {
                    return userInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public KhachHang getById(int id)
        {
            try
            {
                var KhachHang = ApplicationDbContext.KhachHang.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (KhachHang != null)
                {
                    return KhachHang;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }


        }


        public bool checkexistTaiKhoan(string tk, int iduser)
        {
            try
            {
                var user = ApplicationDbContext.KhachHang.Count(x => x.Email.Equals(tk) && x.Id != iduser);
                if (user > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
        }

        public bool checkexistEmail(string e, int iduser)
        {
            try
            {
                var user = ApplicationDbContext.KhachHang.Count(x => x.Email.Equals(e) && x.Id != iduser);
                if (user > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
        }

        public bool checkexistSDT(string sdt, int iduser)
        {
            try
            {
                var user = ApplicationDbContext.KhachHang.Count(x => x.DienThoai.Equals(sdt) && x.Id != iduser);
                if (user > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
        }

        public List<KhachHang> getAll()
        {
            try
            {
                var lisKhachHang = ApplicationDbContext.KhachHang.ToList();
                if (lisKhachHang != null)
                {
                    return lisKhachHang;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public int InsertButReturnId(KhachHang model)
        {
            try
            {
                ApplicationDbContext.KhachHang.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(KhachHang model)
        {
            try
            {
                ApplicationDbContext.KhachHang.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(KhachHang model)
        {
            try
            {
                var old = getById(model.Id);
                old.TenKhach = model.TenKhach;
                //old.Email = model.Email;
                old.Password = model.Password;
                old.DienThoai = model.DienThoai;
                //old.NgayTao = model.NgayTao;
                old.NgaySua = model.NgaySua;
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var model = getById(id);
                ApplicationDbContext.KhachHang.Remove(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
