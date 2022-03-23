using lap4_NguyenQuocTu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace lap4_NguyenQuocTu.Controllers
{
    public class GIoHangController : Controller
    {
        // GET: GIoHang
        MyDataDataContext dt = new MyDataDataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lst = Session["Giohang"] as List<Giohang>;
            if (lst == null)
            {
                lst = new List<Giohang>();
                Session["Giohang"] = lst;
            }
            return lst;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> lst = Laygiohang();
            Giohang sanpham = lst.Find(n => n.Masach == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lst.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.soluong++;
                return Redirect(strURL);
            }
        }
        private int TongSOluong()
        {
            int tsl = 0;
            List<Giohang> lst = Session["Giohang"] as List<Giohang>;
            if (lst !=null)
            {
                tsl = lst.Sum(n => n.soluong);
            }
            return tsl;
        }
        private int Tongsoluongsanpham()
        {
            int tsl = 0;
            List<Giohang> lst = Session["Giohang"] as List<Giohang>;
            if (lst != null)
            {
                tsl = lst.Count;
            }
            return tsl;
        }
        private double Tongtien()
        {
            double tt = 0;
            List<Giohang> lst = Session["Giohang"] as List<Giohang>;
            if(lst != null)
            {
                tt = lst.Sum(n => n.thanhtien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lst = Laygiohang();
            ViewBag.Tongsoluong = TongSOluong();
            ViewBag.Tongtien = Tongtien();
            ViewBag.Tongsoluongsanpham = Tongsoluongsanpham();
            ViewBag.Message = Session["Message"];
            ViewBag.AlertStatus = Session["AlertStatus"];
            
            Session.Remove("Message");
            Session.Remove("AlertStatus");

            return View(lst);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSOluong();
            ViewBag.Tongtien = Tongtien();
            ViewBag.Tongsoluongsanpham = Tongsoluongsanpham();
            return PartialView();
        }
        public ActionResult XoaGiohang(int id)
        {
            List<Giohang> lst = Laygiohang();
            Giohang sanpham = lst.SingleOrDefault(n => n.Masach == id);
            if(sanpham != null)
            {
                lst.RemoveAll(n => n.Masach == id);
                return RedirectToAction("Giohang");
            }
            return RedirectToAction("Giohang");
        }
        
        public ActionResult CapNhatgiohang(int id, FormCollection collection)
        {
            List<Giohang> lst = Laygiohang();
            Giohang sanpham = lst.SingleOrDefault(n => n.Masach == id);
            
            if (sanpham !=null )
            {
                Sach sach = dt.Saches.FirstOrDefault(n => n.masach == id);
                int soluon = int.Parse(collection["txtSolg"].ToString());
                if(soluon > sach.soluongton)
                {
                    Session["Message"] = "Không đủ số lượng";
                    return RedirectToAction("Giohang");
                }
                sanpham.soluong = soluon;
            }
            
            return RedirectToAction("Giohang");
        }
        public ActionResult XoatatcaGiohang()
        {
            List<Giohang> lst = Laygiohang();
            lst.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
           if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Giohang> lst = Laygiohang();
            ViewBag.Tongsoluong = TongSOluong();
            ViewBag.Tongtien = Tongtien();
            ViewBag.Tongsoluongsanpham = Tongsoluongsanpham();
            ViewBag.MessageEx = Session["MessageEx"];
            return View(lst);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();
            List<Giohang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            if(dh.ngaygiao<dh.ngaydat)
            {
                Session["MessageEx"] = "Ngay giao phải sau ngày đặt";
                return RedirectToAction("DatHang");
            }
            else
            {
                dt.DonHangs.InsertOnSubmit(dh);
                dt.SubmitChanges();
            }
            
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.Masach;
                ctdh.soluong = item.soluong;
                ctdh.gia = (decimal)item.giaban;
                s = dt.Saches.Single(n => n.masach == item.Masach);
                s.soluongton -= ctdh.soluong;
                
                dt.SubmitChanges();

                dt.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            dt.SubmitChanges();
            
            Session["Giohang"] = null;


            try
            {
                var senderEmail = new MailAddress("quoctupdn@gmail.com", "Nguyễn Quốc Tú");
                var receiverEmail = new MailAddress(kh.email, "Receiver");
                var password = "QuocTu2907";
                var sub = "Hello";
                var body = "Đơn hàng đã được xác nhận";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
                return RedirectToAction("XacnhanDonhang", "GioHang");

            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }
    }
}