using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Config
{
    public class CommonConfig
    {
        public enum CommonResponse
        {
            [Description("Thành công.")]
            Success = 200,
            [Description("Thất bại.")]
            FailFromClient400 = 400,
            [Description("Dữ liệu đã bị xóa hoặc không tồn tại.")]
            FailFromClient404 = 404,
            [Description("Dữ liệu đang sử dụng, không thể xóa.")]
            FailFromClient405 = 405,
            [Description("Đã xảy ra sự cố, Vui lòng thử lại sau.")]
            FailFromServer = 500,
        }

        public enum CommonStatus
        {
            [Description("Hoạt động.")]
            Normal = 1,
            [Description("Ẩn.")]
            Disabled = 0,
            [Description("Đã xóa.")]
            Delete = -1
        }

        public enum BanStatus
        {
            [Description("Hoạt động.")]
            Trong = 1,
            [Description("Bận.")]
            Ban = 2,
            [Description("Được đặt.")]
            DuocDat = 3,
            [Description("Hỏng.")]
            Hong = 4
        }

        public enum HoaDonStatus
        {
            [Description("Hủy.")]
            Huy = -1,
            [Description("Chờ thanh toán.")]
            ChoThanhToan = 0,
            [Description("Đã thanh toán.")]
            DaThanhToan = 1
        }

        public enum HinhThucThanhToan
        {
            [Description("Tiền mặt.")]
            Cash = 1,
            [Description("VnPay.")]
            VnPay = 2
        }

        public static Dictionary<int, string> HoaDonStatusToDictionary = new Dictionary<int, string>()
        {
            { (int)HoaDonStatus.Huy, HoaDonStatus.Huy.GetDescription() },
            { (int)HoaDonStatus.ChoThanhToan, HoaDonStatus.ChoThanhToan.GetDescription() },
            { (int)HoaDonStatus.DaThanhToan, HoaDonStatus.DaThanhToan.GetDescription() },
        };

        public static Dictionary<int, string> HinhThucThanhToanToDictionary = new Dictionary<int, string>()
        {
            { (int)HinhThucThanhToan.Cash, HinhThucThanhToan.Cash.GetDescription() },
            { (int)HinhThucThanhToan.VnPay, HinhThucThanhToan.VnPay.GetDescription() },
        };

        public static Dictionary<int, string> BanStatusToDictionary = new Dictionary<int, string>()
        {
            { (int)BanStatus.Trong, "FFFFFF/000000" },
            { (int)BanStatus.Ban, "FF0000/FFFFFF" },
            { (int)BanStatus.DuocDat, "FFFF00/000000" },
            { (int)BanStatus.Hong, "000000/FFFFFF" }
        };

        public static Dictionary<int, string> BanStatusToName = new Dictionary<int, string>()
        {
            { (int)BanStatus.Trong, EnumHelper.GetDescription(BanStatus.Trong) },
            { (int)BanStatus.Ban, EnumHelper.GetDescription(BanStatus.Ban) },
            { (int)BanStatus.DuocDat, EnumHelper.GetDescription(BanStatus.DuocDat) },
            { (int)BanStatus.Hong, EnumHelper.GetDescription(BanStatus.Hong) }
        };
    }
    public static class EnumHelper
    {
        public static string GetDescription(this System.Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

    
}
