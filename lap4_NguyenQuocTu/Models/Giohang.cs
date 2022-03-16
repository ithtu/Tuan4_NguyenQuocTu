using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace lap4_NguyenQuocTu.Models
{
    public class Giohang
    {
        MyDataDataContext dt = new MyDataDataContext();
        public int Masach { get; set; }
        [Display(Name = "Tên sách")]
        public string TenSach { get; set; }
        [Display(Name = "Ảnh bìa")]
        public string hinh { get; set; }
        [Display(Name = "Giá bán")]
        public Double giaban { get; set; }
        [Display(Name = "Số lượng")]
        public int soluong { get; set; }
        [Display(Name = "Thành tiền")]
        public Double thanhtien
        {
            get { return soluong * giaban; }
        }
        public Giohang(int d)
        {
            Masach = d;
            Sach sach = dt.Saches.Single(n => n.masach == Masach);
            TenSach = sach.tensach;
            hinh = sach.hinh;
            giaban = double.Parse(sach.giaban.ToString());
            soluong = 1;
        }
    }
}