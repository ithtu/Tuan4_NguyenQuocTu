using lap4_NguyenQuocTu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lap4_NguyenQuocTu.Controllers
{
    public class TheLoaiController : Controller
    {
        // GET: TheLoai
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult ListTheLoai()
        {
            var all_theloai = from tl in data.TheLoais select tl;
            return View(all_theloai);
        }
        public ActionResult Detail(int id)
        {
            var D_sach = data.TheLoais.Where(m => m.maloai == id).First();
            return View(D_sach);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, TheLoai tl)
        {
            var E_tenTL = collection["tenloai"];
            
            if (string.IsNullOrEmpty(E_tenTL))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                tl.tenloai = E_tenTL.ToString();
                
                data.TheLoais.InsertOnSubmit(tl);
                data.SubmitChanges();
                return RedirectToAction("ListTheLoai");
            }
            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var E_theloai = data.TheLoais.First(m => m.maloai == id);
            return View(E_theloai);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_theloai = data.TheLoais.First(m => m.maloai == id);
            var E_tenloai = collection["tenloai"];
            E_theloai.maloai = id;
            if (string.IsNullOrEmpty(E_tenloai))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_theloai.tenloai = E_tenloai;
               
                UpdateModel(E_theloai);
                data.SubmitChanges();
                return RedirectToAction("ListTheLoai");
            }
            return this.Edit(id);
        }
        public ActionResult Delete(int id)
        {
            var D_tl = data.TheLoais.First(m => m.maloai == id);
            return View(D_tl);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_tl = data.TheLoais.Where(m => m.maloai == id).First();
            data.TheLoais.DeleteOnSubmit(D_tl);
            data.SubmitChanges();
            return RedirectToAction("ListTheLoai");
        }
    }
}