using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class NhanViens : ApplicationDbContexts
    {

        public NhanVien getByEmail(string username)
        {
            try
            {
                var userInfo = ApplicationDbContext.NhanVien.Where(x => x.Email.Equals(username)).FirstOrDefault();
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

        public NhanVien getById(int id)
        {
            try
            {
                var NhanVien = ApplicationDbContext.NhanVien.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (NhanVien != null)
                {
                    return NhanVien;
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
                var user = ApplicationDbContext.NhanVien.Count(x => x.Email.Equals(tk) && x.Id != iduser);
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
                var user = ApplicationDbContext.NhanVien.Count(x => x.Email.Equals(e) && x.Id != iduser);
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
                var user = ApplicationDbContext.NhanVien.Count(x => x.DienThoai.Equals(sdt) && x.Id != iduser);
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

        public List<NhanVien> getAll()
        {
            try
            {
                var lisNhanVien = ApplicationDbContext.NhanVien.ToList();
                if (lisNhanVien != null)
                {
                    return lisNhanVien;
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

        public int InsertButReturnId(NhanVien model)
        {
            try
            {
                ApplicationDbContext.NhanVien.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(NhanVien model)
        {
            try
            {
                ApplicationDbContext.NhanVien.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(NhanVien model)
        {
            try
            {
                var old = getById(model.Id);
                old.TenNhanVien = model.TenNhanVien;
                old.Email = model.Email;
                old.Password = model.Password;
                old.DienThoai = model.DienThoai;
                old.NgaySinh = model.NgaySinh;
                old.QueQuan = model.QueQuan;
                old.NgayTao = model.NgayTao;
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
                ApplicationDbContext.NhanVien.Remove(model);
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

