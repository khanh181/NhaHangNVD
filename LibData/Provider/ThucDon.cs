using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;
using LibData.Models;

namespace LibData.Provider
{
    public class ThucDons : ApplicationDbContexts
    {

        public ThucDon getById(int id)
        {
            try
            {
                var ThucDon = ApplicationDbContext.ThucDon.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (ThucDon != null)
                {
                    return ThucDon;
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


        public bool checkexistThucDon(string ten, int idCheck)
        {
            try
            {
                var ThucDon = ApplicationDbContext.ThucDon.Count(x => x.TenThucDon.Equals(ten) && x.Id != idCheck);
                if (ThucDon > 0)
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

        public List<ThucDon> getAll()
        {
            try
            {
                var lisThucDon = ApplicationDbContext.ThucDon.ToList();
                if (lisThucDon != null)
                {
                    return lisThucDon;
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

        public async Task<List<MenuViewData>> getAllAsync()
        {
            try
            {
                var lisThucDon = ApplicationDbContext.ThucDon.OrderByDescending(x => x.NgayTao).ToList();
                if (lisThucDon != null)
                {
                    var result = new List<MenuViewData>();
                    foreach (var item in lisThucDon)
                    {
                        var temp = new MenuViewData();
                        temp.ThucDon = item;
                        temp.MonAns = item.MonAn.OrderByDescending(x => x.NgayTao).Take(20).ToList();
                        result.Add(temp);
                    }
                    return result;
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


        public bool Insert(ThucDon model)
        {
            try
            {
                ApplicationDbContext.ThucDon.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(ThucDon model)
        {
            try
            {
                var old = getById(model.Id);
                old.TenThucDon = model.TenThucDon;
                old.GioiThieu = model.GioiThieu;
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
                ApplicationDbContext.ThucDon.Remove(model);
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
