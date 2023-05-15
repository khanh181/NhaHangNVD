using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Models.TaiChinh
{
    public class ThanhToanHoaDonModel
    {
        // info pay
        public string orderCategory { get; set; } = "";
        public long txtAmount { get; set; }
        public string txtOrderDesc { get; set; } = "";
        public string cboBankCode { get; set; } = "";
        public string cboLanguage { get; set; } = "";
        public DateTime txtExpire { get; set; }

        // info bill
        public string txt_billing_fullname { get; set; } = "";
        public string txt_billing_email { get; set; } = "";
        public string txt_billing_mobile { get; set; } = "";
        public string txt_billing_addr1 { get; set; } = "";
        public int txt_postalcode { get; set; }
        public string txt_bill_city { get; set; } = "";
        public string txt_bill__state { get; set; } = "";
        public string txt_bill_country { get; set; } = "";

        // info ship
        public string txt_ship_fullname { get; set; } = "";
        public string txt_ship_email { get; set; } = "";
        public string txt_ship_mobile { get; set; } = "";
        public string txt_ship_addr1 { get; set; } = "";
        public int txt_ship_postalcode { get; set; }
        public string txt_ship_city { get; set; } = "";
        public string txt_ship_state { get; set; } = "";
        public string txt_ship_country { get; set; } = "";

        // info invoice
        public string txt_inv_customer { get; set; } = "";
        public string txt_inv_company { get; set; } = "";
        public string txt_inv_addr1 { get; set; } = "";
        public int txt_inv_taxcode { get; set; }
        public string cbo_inv_type { get; set; } = "";
        public string txt_inv_email { get; set; } = "";
        public string txt_inv_mobile { get; set; } = "";
    }
}
