using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class DatChos : ApplicationDbContexts
    {

        public DatCho getById(int id)
        {
            try
            {
                var DatCho = ApplicationDbContext.DatCho.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (DatCho != null)
                {
                    return DatCho;
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

       
        public List<DatCho> getAll()
        {
            try
            {
                var lisDatCho = ApplicationDbContext.DatCho.ToList();
                if (lisDatCho != null)
                {
                    return lisDatCho;
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

        public int InsertButReturnId(DatCho model)
        {
            try
            {
                ApplicationDbContext.DatCho.Add(model);
                ApplicationDbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public bool Insert(DatCho model)
        {
            try
            {
                ApplicationDbContext.DatCho.Add(model);
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
                ApplicationDbContext.DatCho.Remove(model);
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
