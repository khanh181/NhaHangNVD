using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class NCCs : ApplicationDbContexts
    {

        public NCC getById(int id)
        {
            try
            {
                var NCC = ApplicationDbContext.NCC.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (NCC != null)
                {
                    return NCC;
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

        public bool checkexistSDT(string ten, int idCheck)
        {
            try
            {
                var NCC = ApplicationDbContext.NCC.Count(x => x.SDT.Equals(ten) && x.Id != idCheck);
                if (NCC > 0)
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

        public List<NCC> getAll()
        {
            try
            {
                var lisNCC = ApplicationDbContext.NCC.ToList();
                if (lisNCC != null)
                {
                    return lisNCC;
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

        public int InsertButReturnId(NCC model)
        {
            try
            {
                ApplicationDbContext.NCC.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(NCC model)
        {
            try
            {
                ApplicationDbContext.NCC.Add(model);
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
                ApplicationDbContext.NCC.Remove(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(LibData.NCC model)
        {
            try
            {
                var old = getById(model.Id);
                old.TenToChuc = model.TenToChuc;
                old.SDT = model.SDT;
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
    }
}
