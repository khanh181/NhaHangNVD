using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class CongThucMonAns : ApplicationDbContexts
    {

        public CongThucMonAn getById(int id)
        {
            try
            {
                var CongThucMonAn = ApplicationDbContext.CongThucMonAn.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (CongThucMonAn != null)
                {
                    return CongThucMonAn;
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
        public bool checkexistCTMA(int idVatTu, int idMonAn, int idCheck)
        {
            try
            {
                var NCC = ApplicationDbContext.CongThucMonAn.Count(x => x.IdVatTu.Equals(idVatTu) && x.IdMonAn.Equals(idMonAn) && x.Id != idCheck);
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

        public List<CongThucMonAn> getAll()
        {
            try
            {
                var lisCongThucMonAn = ApplicationDbContext.CongThucMonAn.ToList();
                if (lisCongThucMonAn != null)
                {
                    return lisCongThucMonAn;
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

        public List<CongThucMonAn> getAllByIdMonAn(int idMonAn)
        {
            try
            {
                var lisCongThucMonAn = ApplicationDbContext.CongThucMonAn.Where(x => x.IdMonAn.Equals(idMonAn)).ToList();
                if (lisCongThucMonAn != null)
                {
                    return lisCongThucMonAn;
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

        public int InsertButReturnId(CongThucMonAn model)
        {
            try
            {
                ApplicationDbContext.CongThucMonAn.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(CongThucMonAn model)
        {
            try
            {
                ApplicationDbContext.CongThucMonAn.Add(model);
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
                ApplicationDbContext.CongThucMonAn.Remove(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(LibData.CongThucMonAn model)
        {
            try
            {
                var old = getById(model.Id);
                old.IdVatTu = model.IdVatTu;
                old.SoLuong = model.SoLuong;
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
