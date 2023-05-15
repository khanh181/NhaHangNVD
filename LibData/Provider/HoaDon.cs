using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class HoaDons : ApplicationDbContexts
    {

        public HoaDon getById(int id)
        {
            try
            {
                var HoaDon = ApplicationDbContext.HoaDon.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (HoaDon != null)
                {
                    return HoaDon;
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

        public List<HoaDon> getAllHoaDonByIdBan(int idBan)
        {
            try
            {
                var HoaDons = ApplicationDbContext.HoaDon.Where(x => x.TinhTrang.Equals((int)CommonConfig.HoaDonStatus.ChoThanhToan)
                                        && x.IdBanAn.Equals(idBan)
                                        && !x.ThoiGianDi.HasValue
                                        ).OrderByDescending(x => x.ThoiGianDen).ToList();
                if (HoaDons != null)
                {
                    return HoaDons;
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

        /*public bool checkexistSDT(string ten, int idCheck)
        {
            try
            {
                var HoaDon = ApplicationDbContext.HoaDon.Count(x => x.TenBan.Equals(ten) && x.Id != idCheck);
                if (HoaDon > 0)
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
        }*/

        public List<HoaDon> getAll()
        {
            try
            {
                var lisHoaDon = ApplicationDbContext.HoaDon.ToList();
                if (lisHoaDon != null)
                {
                    return lisHoaDon;
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

        public int InsertButReturnId(HoaDon model)
        {
            try
            {
                ApplicationDbContext.HoaDon.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(HoaDon model)
        {
            try
            {
                ApplicationDbContext.HoaDon.Add(model);
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
                ApplicationDbContext.HoaDon.Remove(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(LibData.HoaDon model)
        {
            try
            {
                var old = getById(model.Id);
                old.IdBanAn = model.IdBanAn;
                old.SoLuongNguoi = model.SoLuongNguoi;
                old.ThoiGianDen = model.ThoiGianDen;
                old.ThoiGianDi = model.ThoiGianDi;
                old.YeuCauDacBiet = model.YeuCauDacBiet;
                old.CodeKM = model.CodeKM;
                old.TienKM = model.TienKM;
                old.TongTien = model.TongTien;
                old.HinhThucThanhToan = model.HinhThucThanhToan;
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
