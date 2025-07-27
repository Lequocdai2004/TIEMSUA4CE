using BanSua4CE.Data;
using BanSua4CE.Helpers;
using BanSua4CE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BanSua4CE.Controllers
{
    public class QLSPController : Controller
    {
        ShopBanSuaContext _context = new ShopBanSuaContext();
        private readonly IWebHostEnvironment _webHostEnvironment;

       
        public async Task<IActionResult> Index(string searchString, string trangThaiLoc)
        {
            var sanphams = from sp in _context.SanPhams
                           select sp;

            if (!string.IsNullOrEmpty(searchString))
            {
                sanphams = sanphams.Where(sp =>
                    sp.TenSp.Contains(searchString) ||
                    sp.MoTa.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(trangThaiLoc))
            {
                bool isActive = trangThaiLoc == "1"; // 1 hoạt động , 0 không hoạt động
                sanphams = sanphams.Where(sp => sp.Status == isActive);
            }
            // 👉 Thêm dòng này để hiển thị sản phẩm mới nhất lên đầu
            sanphams = sanphams.OrderByDescending(sp => sp.IdSp);

            ViewData["CurrentFilter"] = searchString;
            ViewData["TrangThaiLoc"] = trangThaiLoc;
            return View(await sanphams.ToListAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SanPham = new SelectList(_context.SanPhams.ToList(), "IdSP", "TenSp");
            ViewBag.IdDanhMuc = new SelectList(_context.DanhMucs, "IdDanhMuc", "TenDanhMuc");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SanPham sanpham)
        {
            if (ModelState.IsValid)
            {
                var sp = new SanPham
                {
                    TenSp = sanpham.TenSp,
                    DonGia = sanpham.DonGia,
                    MoTa = sanpham.MoTa,
                    XuatXu = sanpham.XuatXu,
                    Status = true,
                    IdDanhMuc = sanpham.IdDanhMuc,
                    HinhAnh = sanpham.HinhAnh
                };

                _context.SanPhams.Add(sp);
                _context.SaveChanges();
                return RedirectToAction("Index"); //danh sách sp
            }
            ViewBag.IdDanhMuc = new SelectList(_context.DanhMucs, "IdDanhMuc", "TenDanhMuc", sanpham.IdDanhMuc);
            return View(sanpham);
        }



   


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewBag.SanPham = new SelectList(_context.SanPhams.ToList(), "IdSP", "TenSp");
            ViewBag.IdDanhMuc = new SelectList(_context.DanhMucs, "IdDanhMuc", "TenDanhMuc");
            var sp = _context.SanPhams.Find(id);
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SanPham sanpham, IFormFile hinhanh, string HinhAnhCu)
        {
            if (ModelState.IsValid)
            {
                var sp = _context.SanPhams.Find(sanpham.IdSp); // Tìm sản phẩm theo Id
                if (sp == null)
                {
                    return NotFound();
                }

                // Cập nhật các thuộc tính của sản phẩm
                sp.TenSp = sanpham.TenSp;
                sp.DonGia = sanpham.DonGia;
                sp.MoTa = sanpham.MoTa;
                sp.XuatXu = sanpham.XuatXu;
                sp.Status = sanpham.Status;
                sp.IdDanhMuc = sanpham.IdDanhMuc;

                // Xử lý hình ảnh
                if (hinhanh != null && hinhanh.Length > 0)
                {
                    // Nếu có hình ảnh mới → lưu hình ảnh
                    var tenFile = Path.GetFileName(hinhanh.FileName);
                    var duongDan = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/hinhanh", tenFile);

                    using (var stream = new FileStream(duongDan, FileMode.Create))
                    {
                        await hinhanh.CopyToAsync(stream);
                    }

                    sp.HinhAnh = tenFile; // Gán tên file mới
                }
                else
                {
                    sp.HinhAnh = HinhAnhCu; // Giữ ảnh cũ
                }

                _context.SanPhams.Update(sp);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); // Quay lại danh sách sản phẩm
            }

            ViewBag.IdDanhMuc = new SelectList(_context.DanhMucs, "IdDanhMuc", "TenDanhMuc", sanpham.IdDanhMuc);
            return View(sanpham);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sp = _context.SanPhams.Find(id);
            if (sp == null)
            {
                return NotFound();
            }

            return View(sp);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            var sp = _context.SanPhams.Find(id);
            if (sp == null)
            {
                return NotFound();
            }

            _context.SanPhams.Remove(sp);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sp = _context.SanPhams
                .Include(s => s.IdDanhMucNavigation) // Assuming you have a navigation property DanhMuc
                .FirstOrDefault(s => s.IdSp == id);

            if (sp == null)
            {
                return NotFound();
            }

            return View(sp);
        }


        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.IdSp == id);
        }


       

    }
}
