using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;


namespace LibData.Provider
{
    public class ChiNhanhs : ApplicationDbContexts
    {

        public ChiNhanh getById(int id)
        {
            try
            {
                var ChiNhanh = ApplicationDbContext.ChiNhanh.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (ChiNhanh != null)
                {
                    return ChiNhanh;
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


        public bool checkexistChiNhanh(string ten, int idCheck)
        {
            try
            {
                var ChiNhanh = ApplicationDbContext.ChiNhanh.Count(x => x.Ten.Equals(ten) && x.Id != idCheck);
                if (ChiNhanh > 0)
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

        public List<ChiNhanh> getAll()
        {
            try
            {
                var lisChiNhanh = ApplicationDbContext.ChiNhanh.ToList();
                if (lisChiNhanh != null)
                {
                    return lisChiNhanh;
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


        public bool Insert(ChiNhanh model)
        {
            try
            {
                ApplicationDbContext.ChiNhanh.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(ChiNhanh model)
        {
            try
            {
                var old = getById(model.Id);
                old.Ten = model.Ten;
                old.DiaChi = model.DiaChi;
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
                ApplicationDbContext.ChiNhanh.Remove(model);
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
