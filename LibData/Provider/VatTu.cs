using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;


namespace LibData.Provider
{
    public class VatTus : ApplicationDbContexts
    {

        public VatTu getById(int id)
        {
            try
            {
                var VatTu = ApplicationDbContext.VatTu.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (VatTu != null)
                {
                    return VatTu;
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


        public bool checkexistVatTu(string ten, int idCheck)
        {
            try
            {
                var VatTu = ApplicationDbContext.VatTu.Count(x => x.TenVT.Equals(ten) && x.Id != idCheck);
                if (VatTu > 0)
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

        public List<VatTu> getAll()
        {
            try
            {
                var lisVatTu = ApplicationDbContext.VatTu.ToList();
                if (lisVatTu != null)
                {
                    return lisVatTu;
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


        public bool Insert(VatTu model)
        {
            try
            {
                ApplicationDbContext.VatTu.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(VatTu model)
        {
            try
            {
                var old = getById(model.Id);
                old.TenVT = model.TenVT;
                old.SoLuong = model.SoLuong;
                old.DonVi = model.DonVi;
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
                ApplicationDbContext.VatTu.Remove(model);
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
