using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Models.TaiChinh
{
    public class TruyVanKetQuaGiaoDichModel
    {
        public string orderId { get; set; } = "";
        public DateTime payDate { get; set; }
        public int isASC { get; set; } = 0;
        public int size { get; set; } = 10;
        public int? page { get; set; } = 1;
        public string sortBy { get; set; } = "CreateAt";
        public string searchTenBanAn { get; set; } = null;
       
        public bool searchChanged { get; set; } = false;

    }
}
