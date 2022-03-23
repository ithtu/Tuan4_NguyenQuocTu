using lap4_NguyenQuocTu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lap4_NguyenQuocTu.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        MyDataDataContext dt = new MyDataDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang KH)
        {
            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var matkhauxacnhan = collection["matkhauxacnhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:dd/MM/yyyy}", collection["ngaysinh"]);
            if (String.IsNullOrEmpty(matkhauxacnhan))
            {
                ViewData["NhapMKXN"] = "phai nhap mat khau xac nhan";
                
            }
            else
            {
                if (!matkhau.Equals(matkhauxacnhan))
                {
                    ViewData["MatKhauGiongNhau"] = "Mat khau va mat khau xac nhan phai giong nhau";
                }
                else
                {
                    KH.hoten = hoten;
                    KH.tendangnhap = tendangnhap;
                    KH.matkhau = matkhau;
                    KH.email = email;
                    KH.diachi = diachi;
                    KH.dienthoai = dienthoai;
                    KH.ngaysinh = DateTime.Parse(ngaysinh);

                    dt.KhachHangs.InsertOnSubmit(KH);
                    dt.SubmitChanges();

                    return RedirectToAction("DangNhap");
                }
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            KhachHang kh = dt.KhachHangs.SingleOrDefault(n => n.tendangnhap == tendangnhap && n.matkhau == matkhau);
            if(kh != null)
            {
                ViewBag.ThongBao = "Chuc Mung dang nhap thanh cong";    
                Session["TaiKhoan"] = kh;
            }
            else
            {
                ViewBag.ThongBao = "Ten dang nhap hoac mat khau khong dung";
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }
    }
}