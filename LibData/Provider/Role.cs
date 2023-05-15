using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;

namespace LibData.Provider
{
    public class Roles : ApplicationDbContexts
    {

        public Role getById(int id)
        {
            try
            {
                var Role = ApplicationDbContext.Role.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (Role != null)
                {
                    return Role;
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


        public bool checkexistRolename(string Rolename, int idCheck)
        {
            try
            {
                var Role = ApplicationDbContext.Role.Count(x => x.Rolename.Equals(Rolename) && x.Id != idCheck);
                if (Role > 0)
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
        public bool checkexistRolecode(string Rolecode, int idCheck)
        {
            try
            {
                var Role = ApplicationDbContext.Role.Count(x => x.Rolecode.Equals(Rolecode) && x.Id != idCheck);
                if (Role > 0)
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

        public List<Role> getAll()
        {
            try
            {
                var lisRole = ApplicationDbContext.Role.ToList();
                if (lisRole != null)
                {
                    return lisRole;
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


        public bool Insert(Role model)
        {
            try
            {
                ApplicationDbContext.Role.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(Role model)
        {
            try
            {
                var old = getById(model.Id);
                old.Rolename = model.Rolename;
                old.Rolecode = model.Rolecode;
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
                ApplicationDbContext.Role.Remove(model);
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
