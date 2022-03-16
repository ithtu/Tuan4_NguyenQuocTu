using lap4_NguyenQuocTu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    Session["Message"] = "Khong du so luong";
                    Session["AlertStatus"] = "danger";
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
        public ActionResult DatHang()
        {
            List<Giohang> lst = Laygiohang();
            foreach(var item in lst)
            {
                var sach = dt.Saches.FirstOrDefault(m => m.masach == item.Masach);
                sach.soluongton -= item.soluong;
            }
            dt.SubmitChanges();
            lst.Clear();
            return RedirectToAction("Giohang");
        }
    }
}