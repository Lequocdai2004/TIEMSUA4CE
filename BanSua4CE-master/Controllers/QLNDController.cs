using BanSua4CE.Data;
using BanSua4CE.Helpers;
using BanSua4CE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BanSua4CE.Controllers
{
    public class QLNDController : Controller
    {
        private readonly ShopBanSuaContext _context;

        public QLNDController (ShopBanSuaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var kh = _context.KhachHangs.ToList();
            return View(kh);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(KhachHang khachhang)
        {
            var kh = new KhachHangVM
            {
                TenKh = khachhang.TenKh,
                Email = khachhang.Email,
                DiaChi = khachhang.DiaChi,
                SoDienThoai = khachhang.SoDienThoai,
                MatKhau = khachhang.MatKhau.ToSHA512Hash("MatKhau")
            };
            _context.KhachHangs.Add(khachhang);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var khachhang = _context.KhachHangs.Find(id);
            if (khachhang == null)
                return NotFound();

            return View(khachhang);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(KhachHang khachhang)
        {
            if (ModelState.IsValid)
            {
                var existingKhachHang = _context.KhachHangs.Find(khachhang.IdKh);
                if (existingKhachHang == null)
                    return NotFound();

                // Cập nhật các trường
                existingKhachHang.TenKh = khachhang.TenKh;
                existingKhachHang.Email = khachhang.Email;
                existingKhachHang.DiaChi = khachhang.DiaChi;
                existingKhachHang.SoDienThoai = khachhang.SoDienThoai;
                existingKhachHang.MatKhau = khachhang.MatKhau.ToSHA256Hash("MatKhau");
                // Nếu để trống, giữ nguyên mật khẩu cũ
                _context.KhachHangs.Update(existingKhachHang);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(khachhang); // nếu có lỗi thì trả về View
        }



        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kh = _context.KhachHangs.Find(id);
            if (kh == null)
            {
                return NotFound();
            }

            return View(kh);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            var kh = _context.KhachHangs.Find(id);
            if (kh == null)
            {
                return NotFound();
            }

            _context.KhachHangs.Remove(kh);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var khachhang = _context.KhachHangs.FirstOrDefault(kh => kh.IdKh == id);

            if (khachhang == null)
                return NotFound();

            return View(khachhang);
        }
    }
}
