using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class KhuyenMais : ApplicationDbContexts
    {

        public KhuyenMai getById(int id)
        {
            try
            {
                var KhuyenMai = ApplicationDbContext.KhuyenMai.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (KhuyenMai != null)
                {
                    return KhuyenMai;
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

        public KhuyenMai getByCode(string code)
        {
            try
            {
                var KhuyenMai = ApplicationDbContext.KhuyenMai.Where(x => x.Code.Equals(code)).FirstOrDefault();
                if (KhuyenMai != null)
                {
                    return KhuyenMai;
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

        public bool checkexistSDT(string ma, int idCheck)
        {
            try
            {
                var KhuyenMai = ApplicationDbContext.KhuyenMai.Count(x => x.Code.Equals(ma) && x.Id != idCheck);
                if (KhuyenMai > 0)
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

        public List<KhuyenMai> getAll()
        {
            try
            {
                var lisKhuyenMai = ApplicationDbContext.KhuyenMai.ToList();
                if (lisKhuyenMai != null)
                {
                    return lisKhuyenMai;
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

        public int InsertButReturnId(KhuyenMai model)
        {
            try
            {
                ApplicationDbContext.KhuyenMai.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(KhuyenMai model)
        {
            try
            {
                ApplicationDbContext.KhuyenMai.Add(model);
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
                ApplicationDbContext.KhuyenMai.Remove(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(LibData.KhuyenMai model)
        {
            try
            {
                var old = getById(model.Id);
                old.Code = model.Code;
                old.NoiDung = model.NoiDung;
                old.KMPhanTram = model.KMPhanTram;
                old.KMTienMat = model.KMTienMat;
                old.TinhTrang = model.TinhTrang;
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
