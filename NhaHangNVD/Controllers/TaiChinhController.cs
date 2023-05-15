using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibData.Models.TaiChinh;
using System.Configuration;
using System.Net;
using System.IO;
using NhaHangNVD.Infrastructure;
using LibData.Config;

namespace WebPortal.Controllers
{
    [CustomAuthenticationFilter]
    public class TaiChinhController : Controller
    {
        public ActionResult ThanhToanHoaDon(int id, string codeKM = null)
        {
            try
            {
                var model = new LibData.Provider.HoaDons().getById(id);
                
                var tongTien = (int)new LibData.Provider.NDHoaDons().getTongTienTamTinh(model.Id);
                if(!string.IsNullOrEmpty(codeKM))
                {
                    var km = new LibData.Provider.KhuyenMais().getByCode(codeKM);
                    var tienKM = 0;
                    model.CodeKM = codeKM;
                    if (km.KMTienMat.HasValue)
                    {
                        tienKM += km.KMTienMat.Value;
                        tongTien = tongTien - km.KMTienMat.Value;
                    }
                    if (km.KMPhanTram.HasValue)
                    {
                        tienKM += (tongTien / 100 * km.KMPhanTram.Value);
                        tongTien = tongTien - (tongTien / 100 * km.KMPhanTram.Value);
                    }
                    model.TienKM = tienKM;
                }
                
                model.TongTien = tongTien;
                new LibData.Provider.HoaDons().Update(model);
                ViewBag.TongTien = tongTien;
                
                return View(model);
            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThanhToanHoaDon(ThanhToanHoaDonModel model)
        {
            try
            {
                //Get Config Info
                string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
                string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
                {
                    return Json(new { status = 500, msg = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config!" }, JsonRequestBehavior.AllowGet);
                }
                //Get payment input
                OrderInfo order = new OrderInfo();
                //Save order to db
                order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
                order.Amount = model.txtAmount; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
                order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
                order.OrderDesc = model.txtOrderDesc;
                order.CreatedDate = DateTime.Now;
                string locale = model.cboLanguage;
                //Build URL for VNPAY
                var vnpay = new LibData.Provider.VnPayLibrary();
                var stringProperties = model.GetType().GetProperties()
                          .Where(p => p.PropertyType == typeof(string));

                foreach (var stringProperty in stringProperties)
                {
                    string currentValue = (string)stringProperty.GetValue(model, null);
                    stringProperty.SetValue(model, currentValue, null);
                }
                vnpay.AddRequestData("vnp_Version", LibData.Provider.VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
                if (model.cboBankCode != null && !string.IsNullOrEmpty(model.cboBankCode))
                {
                    vnpay.AddRequestData("vnp_BankCode", model.cboBankCode);
                }
                vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", LibData.Provider.Utils.GetIpAddress());
                if (!string.IsNullOrEmpty(locale))
                {
                    vnpay.AddRequestData("vnp_Locale", locale);
                }
                else
                {
                    vnpay.AddRequestData("vnp_Locale", "vn");
                }
                vnpay.AddRequestData("vnp_OrderInfo", model.txtOrderDesc);
                vnpay.AddRequestData("vnp_OrderType", model.orderCategory); //default value: other
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

                //Add Params of 2.1.0 Version
                vnpay.AddRequestData("vnp_ExpireDate", model.txtExpire.ToString("yyyyMMddHHmmss"));
                //Billing
                vnpay.AddRequestData("vnp_Bill_Mobile", model.txt_billing_mobile);
                vnpay.AddRequestData("vnp_Bill_Email", model.txt_billing_email);
                var fullName = model.txt_billing_fullname;
                if (!String.IsNullOrEmpty(fullName))
                {
                    var indexof = fullName.IndexOf(' ');
                    if (indexof == -1)
                    {
                        vnpay.AddRequestData("vnp_Bill_FirstName", fullName);
                        vnpay.AddRequestData("vnp_Bill_LastName", "");
                    }
                    else
                    {
                        vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
                        vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(fullName.Length - indexof - 1));
                    }
                }
                vnpay.AddRequestData("vnp_Bill_Address", model.txt_inv_addr1);
                vnpay.AddRequestData("vnp_Bill_City", model.txt_bill_city);
                vnpay.AddRequestData("vnp_Bill_Country", model.txt_bill_country);
                vnpay.AddRequestData("vnp_Bill_State", "");

                // Invoice
                vnpay.AddRequestData("vnp_Inv_Phone", model.txt_inv_mobile);
                vnpay.AddRequestData("vnp_Inv_Email", model.txt_inv_email);
                vnpay.AddRequestData("vnp_Inv_Customer", model.txt_inv_customer);
                vnpay.AddRequestData("vnp_Inv_Address", model.txt_inv_addr1);
                vnpay.AddRequestData("vnp_Inv_Company", model.txt_inv_company);
                vnpay.AddRequestData("vnp_Inv_Taxcode", model.txt_inv_taxcode.ToString());
                vnpay.AddRequestData("vnp_Inv_Type", model.cbo_inv_type);

                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                return Json(new { status = 200, msg = paymentUrl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThanhToanTienMat(int id)
        {
            var provider = new LibData.Provider.HoaDons();
            try
            {
                var hd = provider.getById(id);
                if (hd == null)
                {
                    return Json(new { status = 400, msg = "Hóa đơn không tồn tại hoặc đã bị xóa!" }, JsonRequestBehavior.AllowGet);
                }

                hd.TinhTrang = (int)CommonConfig.HoaDonStatus.DaThanhToan;
                hd.HinhThucThanhToan = (int)CommonConfig.HinhThucThanhToan.Cash;
                hd.ThoiGianDi = DateTime.Now;
                
                if (provider.Update(hd))
                {
                    if (!string.IsNullOrEmpty(hd.CodeKM))
                    {
                        var km = new LibData.Provider.KhuyenMais().getByCode(hd.CodeKM);
                        km.TinhTrang = 0;
                        new LibData.Provider.KhuyenMais().Update(km);
                    }
                    return Json(new { status = 200, msg = "Thanh toán thành công!" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { status = 400, msg = "Thanh toán thất bại!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult KetQuaThanhToan(KetQuaThanhToanModel model)
        {
            try
            {
                var hd = new LibData.Provider.HoaDons().getById(int.Parse(model.vnp_OrderInfo.Split(':')[1]));
                new LibData.Provider.VnPayResults().Insert(new LibData.VnPayResult() { Id = Guid.NewGuid(), IdHoaDon = hd.Id, ObjectResult = Newtonsoft.Json.JsonConvert.SerializeObject(model), NgayTao = DateTime.Now });
                if(model.vnp_ResponseCode == "00")
                {
                    hd.TinhTrang = (int)CommonConfig.HoaDonStatus.DaThanhToan;
                    hd.HinhThucThanhToan = (int)CommonConfig.HinhThucThanhToan.VnPay;
                    hd.ThoiGianDi = DateTime.Now;
                    if (new LibData.Provider.HoaDons().Update(hd))
                    {
                        if(!string.IsNullOrEmpty(hd.CodeKM))
                        {
                            var km = new LibData.Provider.KhuyenMais().getByCode(hd.CodeKM);
                            km.TinhTrang = 0;
                            new LibData.Provider.KhuyenMais().Update(km);
                        }    
                    }    
                }    
                return View(model);
            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TruyVanKetQuaGiaoDich()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TruyVanKetQuaGiaoDich(TruyVanKetQuaGiaoDichModel model)
        {
            try
            {
                var vnpayApiUrl = ConfigurationManager.AppSettings["querydr"];
                var vnpHashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
                var vnpTmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];

                //Get payment input
                var vnpay = new LibData.Provider.VnPayLibrary();
                var createDate = DateTime.Now;

                vnpay.AddRequestData("vnp_Version", LibData.Provider.VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "querydr");
                vnpay.AddRequestData("vnp_TmnCode", vnpTmnCode);
                vnpay.AddRequestData("vnp_TxnRef", model.orderId);
                vnpay.AddRequestData("vnp_OrderInfo", "queryDr OrderId:" + model.orderId);
                vnpay.AddRequestData("vnp_TransDate", model.payDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_IpAddr", LibData.Provider.Utils.GetIpAddress());

                var queryDr = vnpay.CreateRequestUrl(vnpayApiUrl, vnpHashSecret);

                var strDatax = "";
                var request = (HttpWebRequest)WebRequest.Create(queryDr);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                    {
                        strDatax = reader.ReadToEnd();
                    }
                return Json(new { status = 200, msg = strDatax }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult YeuCauHoanTien()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult YeuCauHoanTien(YeuCauHoanTienModel model)
        {
            try
            {
                var vnpayApiUrl = ConfigurationManager.AppSettings["querydr"];
                var vnpHashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
                var vnpTmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
                var vnpay = new LibData.Provider.VnPayLibrary();
                var createDate = DateTime.Now;

              
                var amountrf = Convert.ToInt32(model.Amount) * 100;
                vnpay.AddRequestData("vnp_Version", LibData.Provider.VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "refund");
                vnpay.AddRequestData("vnp_TmnCode", vnpTmnCode);

                vnpay.AddRequestData("vnp_TransactionType", model.RefundCategory);
                vnpay.AddRequestData("vnp_CreateBy", model.Email);
                vnpay.AddRequestData("vnp_TxnRef", model.OrderId);
                vnpay.AddRequestData("vnp_Amount", amountrf.ToString());
                vnpay.AddRequestData("vnp_OrderInfo", "REFUND ORDERID:" + model.OrderId);
                vnpay.AddRequestData("vnp_TransDate", model.payDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_IpAddr", LibData.Provider.Utils.GetIpAddress());

                var strDatax = "";

                var refundtUrl = vnpay.CreateRequestUrl(vnpayApiUrl, vnpHashSecret);

                var request = (HttpWebRequest)WebRequest.Create(refundtUrl);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            strDatax = reader.ReadToEnd();
                        }
                return Json(new { status = 200, msg = strDatax }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}


