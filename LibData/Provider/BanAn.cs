using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;
using static LibData.Config.CommonConfig;

namespace LibData.Provider
{
    public class BanAns : ApplicationDbContexts
    {

        public BanAn getById(int id)
        {
            try
            {
                var BanAn = ApplicationDbContext.BanAn.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (BanAn != null)
                {
                    return BanAn;
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
                var BanAn = ApplicationDbContext.BanAn.Count(x => x.TenBan.Equals(ten) && x.Id != idCheck);
                if (BanAn > 0)
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

        public List<BanAn> getAll()
        {
            try
            {
                var lisBanAn = ApplicationDbContext.BanAn.ToList();
                if (lisBanAn != null)
                {
                    return lisBanAn;
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

        public List<BanAn> getAllKhongHong()
        {
            try
            {
                var lisBanAn = ApplicationDbContext.BanAn.Where(x => x.TenBan != "DB" && x.TinhTrang != (int)BanStatus.Hong).ToList();
                if (lisBanAn != null)
                {
                    return lisBanAn;
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

        public List<BanAn> getAllBanTrong()
        {
            try
            {
                var lisBanAn = ApplicationDbContext.BanAn.Where(x => x.TenBan != "DB" && x.TinhTrang.Equals((int)BanStatus.Trong)).ToList();
                if (lisBanAn != null)
                {
                    return lisBanAn;
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


        public int InsertButReturnId(BanAn model)
        {
            try
            {
                ApplicationDbContext.BanAn.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(BanAn model)
        {
            try
            {
                ApplicationDbContext.BanAn.Add(model);
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
                ApplicationDbContext.BanAn.Remove(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(LibData.BanAn model)
        {
            try
            {
                var old = getById(model.Id);
                old.TenBan = model.TenBan;
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
