using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;


namespace LibData.Provider
{
    public class MonAns : ApplicationDbContexts
    {

        public MonAn getById(int id)
        {
            try
            {
                var MonAn = ApplicationDbContext.MonAn.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (MonAn != null)
                {
                    return MonAn;
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


        public bool checkexistMonAn(string ten, int idCheck)
        {
            try
            {
                var MonAn = ApplicationDbContext.MonAn.Count(x => x.TenMon.Equals(ten) && x.Id != idCheck);
                if (MonAn > 0)
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

        public List<MonAn> getAll()
        {
            try
            {
                var lisMonAn = ApplicationDbContext.MonAn.ToList();
                if (lisMonAn != null)
                {
                    return lisMonAn;
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

        public async Task<List<LibData.MonAn>> getSpecialAsync()
        {
            try
            {
                var lisMonAn = ApplicationDbContext.MonAn.Where(x => x.IsDacBiet.HasValue && x.IsDacBiet.Value).OrderByDescending(x => x.NgayTao).Take(5).ToList();
                if (lisMonAn != null)
                {
                    return lisMonAn;
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

        public int InsertButReturnId(MonAn model)
        {
            try
            {
                ApplicationDbContext.MonAn.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(MonAn model)
        {
            try
            {
                ApplicationDbContext.MonAn.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(MonAn model)
        {
            try
            {
                var old = getById(model.Id);
                old.TenMon = model.TenMon;
                old.IdThucDon = model.IdThucDon;
                old.IsDacBiet = model.IsDacBiet;
                old.GioiThieu = model.GioiThieu;
                old.GiaTien = model.GiaTien;
                old.GioiThieuNgan = model.GioiThieuNgan;
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
                ApplicationDbContext.MonAn.Remove(model);
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
