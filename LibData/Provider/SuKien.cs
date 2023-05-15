using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;


namespace LibData.Provider
{
    public class SuKiens : ApplicationDbContexts
    {

        public SuKien getById(int id)
        {
            try
            {
                var SuKien = ApplicationDbContext.SuKien.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (SuKien != null)
                {
                    return SuKien;
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


        public bool checkexistSuKien(string ten, int idCheck)
        {
            try
            {
                var SuKien = ApplicationDbContext.SuKien.Count(x => x.TieuDe.Equals(ten) && x.Id != idCheck);
                if (SuKien > 0)
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

        public List<SuKien> getAll()
        {
            try
            {
                var lisSuKien = ApplicationDbContext.SuKien.ToList();
                if (lisSuKien != null)
                {
                    return lisSuKien;
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

        public async Task<List<LibData.SuKien>> getAllAsync()
        {
            try
            {
                var lisSuKien = ApplicationDbContext.SuKien.OrderByDescending(x => x.NgayTao).ToList();
                if (lisSuKien != null)
                {
                    return lisSuKien;
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

        public bool Insert(SuKien model)
        {
            try
            {
                ApplicationDbContext.SuKien.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(SuKien model)
        {
            try
            {
                var old = getById(model.Id);
                old.TieuDe = model.TieuDe;
                old.ChiPhi = model.ChiPhi;
                old.MoTa = model.MoTa;
                old.HinhAnh = model.HinhAnh;
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
                ApplicationDbContext.SuKien.Remove(model);
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
