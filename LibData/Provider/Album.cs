using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibData.Config;


namespace LibData.Provider
{
    public class Albums : ApplicationDbContexts
    {

        public Album getById(int id)
        {
            try
            {
                var Album = ApplicationDbContext.Album.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (Album != null)
                {
                    return Album;
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


        public bool checkexistAlbum(string ten, int idCheck)
        {
            try
            {
                var Album = ApplicationDbContext.Album.Count(x => x.TenAnh.Equals(ten) && x.Id != idCheck);
                if (Album > 0)
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

        public List<Album> getAll()
        {
            try
            {
                var lisAlbum = ApplicationDbContext.Album.ToList();
                if (lisAlbum != null)
                {
                    return lisAlbum;
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

        public async Task<List<LibData.Album>> getAllAsync()
        {
            try
            {
                var lisAlbum = ApplicationDbContext.Album.Where(x => x.LoaiAnh.Equals("Album")).OrderByDescending(x => x.NgayTao).Take(8).ToList();
                if (lisAlbum != null)
                {
                    return lisAlbum;
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

        public async Task<List<LibData.Album>> getAllDauBepAsync()
        {
            try
            {
                var lisAlbum = ApplicationDbContext.Album.Where(x => x.LoaiAnh.Equals("DauBep")).OrderByDescending(x => x.NgayTao).Take(3).ToList();
                if (lisAlbum != null)
                {
                    return lisAlbum;
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
        public async Task<List<LibData.Album>> getAllAvaAsync()
        {
            try
            {
                var lisAlbum = ApplicationDbContext.Album.Where(x => x.LoaiAnh.Equals("Ava")).OrderByDescending(x => x.NgayTao).Take(5).ToList();
                if (lisAlbum != null)
                {
                    return lisAlbum;
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

        public async Task<List<LibData.Album>> getAllCharAsync()
        {
            try
            {
                var lisAlbum = ApplicationDbContext.Album.Where(x => x.LoaiAnh.Equals("Char")).OrderByDescending(x => x.NgayTao).Take(8).ToList();
                if (lisAlbum != null)
                {
                    return lisAlbum;
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

        public bool Insert(Album model)
        {
            try
            {
                ApplicationDbContext.Album.Add(model);
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(Album model)
        {
            try
            {
                var old = getById(model.Id);
                old.LoaiAnh = model.LoaiAnh;
                old.TenAnh = model.TenAnh;
                old.GhiChu = model.GhiChu;
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
                ApplicationDbContext.Album.Remove(model);
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
