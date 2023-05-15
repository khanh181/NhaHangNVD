using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class VnPayResults : ApplicationDbContexts
    {

        public VnPayResult getById(Guid id)
        {
            try
            {
                var VnPayResult = ApplicationDbContext.VnPayResult.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (VnPayResult != null)
                {
                    return VnPayResult;
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


        public List<VnPayResult> getAll()
        {
            try
            {
                var lisVnPayResult = ApplicationDbContext.VnPayResult.ToList();
                if (lisVnPayResult != null)
                {
                    return lisVnPayResult;
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


        public bool Insert(VnPayResult model)
        {
            try
            {
                ApplicationDbContext.VnPayResult.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                var model = getById(id);
                ApplicationDbContext.VnPayResult.Remove(model);
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
