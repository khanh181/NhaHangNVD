using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class NDHoaDons : ApplicationDbContexts
    {

        public NDHoaDon getById(int id)
        {
            try
            {
                var NDHoaDon = ApplicationDbContext.NDHoaDon.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (NDHoaDon != null)
                {
                    return NDHoaDon;
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

        public bool checkexist(int idHd, int idMa, int idCheck)
        {
            try
            {
                var NDHoaDon = ApplicationDbContext.NDHoaDon.Count(x => x.IdHoaDon.Equals(idHd) && x.IdMonAn.Equals(idMa) && x.Id != idCheck);
                if (NDHoaDon > 0)
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

        public long getTongTienTamTinh(int idHoaDon)
        {
            var list = ApplicationDbContext.NDHoaDon.Where(x => x.IdHoaDon.Equals(idHoaDon)).ToList();
            if (list != null)
            {
                return list.Sum(x => x.Giatien * x.SoLuong);
            }
            else
            {
                return 0;
            }
        }

        public List<NDHoaDon> getAllByIdHoaDon(int idHd)
        {
            try
            {
                var lisNDHoaDon = ApplicationDbContext.NDHoaDon.Where(x => x.IdHoaDon.Equals(idHd)).ToList();
                if (lisNDHoaDon != null)
                {
                    return lisNDHoaDon;
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

        public List<NDHoaDon> getAll()
        {
            try
            {
                var lisNDHoaDon = ApplicationDbContext.NDHoaDon.ToList();
                if (lisNDHoaDon != null)
                {
                    return lisNDHoaDon;
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

        public int InsertButReturnId(NDHoaDon model)
        {
            try
            {
                ApplicationDbContext.NDHoaDon.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(NDHoaDon model)
        {
            try
            {
                ApplicationDbContext.NDHoaDon.Add(model);
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
                ApplicationDbContext.NDHoaDon.Remove(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(LibData.NDHoaDon model)
        {
            try
            {
                var old = getById(model.Id);
                old.Giatien = model.Giatien;
                old.SoLuong = model.SoLuong;
                old.ThanhTien = model.ThanhTien;
                old.NgaySua = model.NgaySua;
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
