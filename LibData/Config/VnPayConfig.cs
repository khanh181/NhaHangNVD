using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Configuration
{
    public class VnPayConfig
    {
        public class OrderCategory
        {
            public static string topup = "topup";
            public const string topup_TEXT = "Nạp tiền điện thoại";
            public static string billpayment = "billpayment";
            public const string billpayment_TEXT = "Thanh toán hóa đơn";
            public static string fashion = "fashion";
            public const string fashion_TEXT = "Thời trang";
            public static string other = "other";
            public const string other_TEXT = "Thanh toán trực tuyến";
        }

        public static Dictionary<string, string> OrderCategoryToDictionary = new Dictionary<string, string>()
        {
            { OrderCategory.topup, OrderCategory.topup_TEXT },
            { OrderCategory.billpayment, OrderCategory.billpayment_TEXT },
            { OrderCategory.fashion, OrderCategory.fashion_TEXT },
            { OrderCategory.other, OrderCategory.other_TEXT }
        };

        public class BankCode
        {
            public static string NULL = "";
            public const string NULL_TEXT = "Không chọn";
            public static string VNPAYQR = "VNPAYQR";
            public const string VNPAYQR_TEXT = "VNPAYQR";
            public static string VNBANK = "VNBANK";
            public const string VNBANK_TEXT = "Ngân hàng nội địa";
            public static string INTCARD = "INTCARD";
            public const string INTCARD_TEXT = "Ngân hàng quốc tế";
            public static string VISA = "VISA";
            public const string VISA_TEXT = "VISA";
            public static string MASTERCARD = "MASTERCARD";
            public const string MASTERCARD_TEXT = "MASTERCARD";
            public static string JCB = "JCB";
            public const string JCB_TEXT = "JCB";
            public static string UPI = "UPI";
            public const string UPI_TEXT = "UPI";
            public static string NCB = "NCB";
            public const string NCB_TEXT = "Ngân hàng NCB";
            public static string SACOMBANK = "SACOMBANK";
            public const string SACOMBANK_TEXT = "Ngân hàng SacomBank";
            public static string EXIMBANK = "EXIMBANK";
            public const string EXIMBANK_TEXT = "Ngân hàng EximBank";
            public static string MSBANK = "MSBANK";
            public const string MSBANK_TEXT = "Ngân hàng MSBANK";
            public static string NAMABANK = "NAMABANK";
            public const string NAMABANK_TEXT = "Ngân hàng NamABank";
            public static string VNMART = "VNMART";
            public const string VNMART_TEXT = "Ví điện tử VNPAY";
            public static string VIETINBANK = "VIETINBANK";
            public const string VIETINBANK_TEXT = "Ngân hàng Vietinbank";
            public static string VIETCOMBANK = "VIETCOMBANK";
            public const string VIETCOMBANK_TEXT = "Ngân hàng VCB";
            public static string HDBANK = "HDBANK";
            public const string HDBANK_TEXT = "Ngân hàng HDBank";
            public static string DONGABANK = "DONGABANK";
            public const string DONGABANK_TEXT = "Ngân hàng Đông Á";
            public static string TPBANK = "TPBANK";
            public const string TPBANK_TEXT = "Ngân hàng TPBank";
            public static string OJB = "OJB";
            public const string OJB_TEXT = "Ngân hàng OceanBank";
            public static string BIDV = "BIDV";
            public const string BIDV_TEXT = "Ngân hàng BIDV";
            public static string TECHCOMBANK = "TECHCOMBANK";
            public const string TECHCOMBANK_TEXT = "Ngân hàng Techcombank";
            public static string VPBANK = "VPBANK";
            public const string VPBANK_TEXT = "Ngân hàng VPBank";
            public static string AGRIBANK = "AGRIBANK";
            public const string AGRIBANK_TEXT = "Ngân hàng Agribank";
            public static string ACB = "ACB";
            public const string ACB_TEXT = "Ngân hàng ACB";
            public static string OCB = "OCB";
            public const string OCB_TEXT = "Ngân hàng Phương Đông";
            public static string SCB = "SCB";
            public const string SCB_TEXT = "Ngân hàng SCB";
        }

        public static Dictionary<string, string> BankCodeToDictionary = new Dictionary<string, string>()
        {
            { BankCode.NULL, BankCode.NULL_TEXT },
            { BankCode.VNPAYQR, BankCode.VNPAYQR_TEXT },
            { BankCode.VNBANK, BankCode.VNBANK_TEXT },
            { BankCode.INTCARD, BankCode.INTCARD_TEXT },
            { BankCode.VISA, BankCode.VISA_TEXT },
            { BankCode.MASTERCARD, BankCode.MASTERCARD_TEXT },
            { BankCode.JCB, BankCode.JCB_TEXT },
            { BankCode.UPI, BankCode.UPI_TEXT },
            { BankCode.NCB, BankCode.NCB_TEXT },
            { BankCode.SACOMBANK, BankCode.SACOMBANK_TEXT },
            { BankCode.EXIMBANK, BankCode.EXIMBANK_TEXT },
            { BankCode.MSBANK, BankCode.MSBANK_TEXT },
            { BankCode.NAMABANK, BankCode.NAMABANK_TEXT },
            { BankCode.VNMART, BankCode.VNMART_TEXT },
            { BankCode.VIETINBANK, BankCode.VIETINBANK_TEXT },
            { BankCode.VIETCOMBANK, BankCode.VIETCOMBANK_TEXT },
            { BankCode.HDBANK, BankCode.HDBANK_TEXT },
            { BankCode.DONGABANK, BankCode.DONGABANK_TEXT },
            { BankCode.TPBANK, BankCode.TPBANK_TEXT },
            { BankCode.OJB, BankCode.OJB_TEXT },
            { BankCode.BIDV, BankCode.BIDV_TEXT },
            { BankCode.TECHCOMBANK, BankCode.TECHCOMBANK_TEXT },
            { BankCode.VPBANK, BankCode.VPBANK_TEXT },
            { BankCode.AGRIBANK, BankCode.AGRIBANK_TEXT },
            { BankCode.ACB, BankCode.ACB_TEXT },
            { BankCode.OCB, BankCode.OCB_TEXT },
            { BankCode.SCB, BankCode.SCB_TEXT },
        };

        public class Language
        {
            public static string vn = "vn";
            public const string vn_TEXT = "Tiếng Việt";
            public static string en = "en";
            public const string en_TEXT = "English";
        }

        public static Dictionary<string, string> LanguageToDictionary = new Dictionary<string, string>()
        {
            { Language.vn, Language.vn_TEXT },
            { Language.en, Language.en_TEXT },
        };

        public class Type
        {
            public static string I = "I";
            public const string I_TEXT = "Cá Nhân";
            public static string O = "O";
            public const string O_TEXT = "Công ty/Tổ chức";
        }

        public static Dictionary<string, string> TypeToDictionary = new Dictionary<string, string>()
        {
            { Type.I, Type.I_TEXT },
            { Type.O, Type.O_TEXT },
        };
    }
}
