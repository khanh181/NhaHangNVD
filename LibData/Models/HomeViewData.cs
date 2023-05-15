using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Models
{
    public class HomeViewData
    {
        public List<MenuViewData> Menus { get; set; }
        public List<LibData.MonAn> MonAnDacBiets { get; set; }
        public List<LibData.SuKien> SuKiens { get; set; }
        public List<LibData.Album> Albums { get; set; }
        public List<LibData.Album> DauBeps { get; set; }
        public List<LibData.Album> Avas { get; set; }
        public List<LibData.Album> Chars { get; set; }
    }

    public class MenuViewData
    {
        public LibData.ThucDon ThucDon { get; set; }
        public List<LibData.MonAn> MonAns { get; set; }
    }

}
