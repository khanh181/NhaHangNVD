using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Models.TaiChinh
{
    public class YeuCauHoanTienModel
    {
        public string OrderId { get; set; } = "";
        public string Amount { get; set; } = "";
        public string RefundCategory { get; set; } = "";
        public DateTime payDate { get; set; }
        public string Email { get; set; } = "";

    }
}
