using BanSua4CE.Data;
using BanSua4CE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BanSua4CE.Controllers
{
    public class QLDMController : Controller
    {
        private ShopBanSuaContext _context;

        public QLDMController(ShopBanSuaContext shopBanSuaContext)
        {
           _context = shopBanSuaContext;
        }

        public IActionResult Index()
        {
            var lstdanhmuc = _context.DanhMucs.ToList();
            return View(lstdanhmuc);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DanhMuc danhmuc)
        {
            if (ModelState.IsValid) 
            {
                var dm = new DanhMucVM
                {
                    IdDanhMuc = danhmuc.IdDanhMuc,
                    TenDanhMuc = danhmuc.TenDanhMuc,
                    TrangThai = true,
                };
                _context.DanhMucs.Add(danhmuc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhmuc);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var danhmuc = _context.DanhMucs.Find(id);
            if (danhmuc == null)
                return NotFound();

            return View(danhmuc);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DanhMuc danhmuc)
        {
            if (ModelState.IsValid)
            {
                var existingDM = _context.DanhMucs.Include(dm => dm.SanPhams)
                                                  .FirstOrDefault(dm => dm.IdDanhMuc == danhmuc.IdDanhMuc);
                if (existingDM == null)
                    return NotFound();

                // Cập nhật thông tin danh mục
                existingDM.TenDanhMuc = danhmuc.TenDanhMuc;
                existingDM.TrangThai = danhmuc.TrangThai;

                // Cập nhật trạng thái hiển thị cho sản phẩm thuộc danh mục
                foreach (var sp in existingDM.SanPhams)
                {
                    sp.Status = danhmuc.TrangThai; // true: hiển thị, false: ẩn
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danhmuc);
        }



        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sp = _context.DanhMucs.Find(id);
            if (sp == null)
            {
                return NotFound();
            }

            return View(sp);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var danhmuc = _context.DanhMucs
                                  .Include(dm => dm.SanPhams)
                                  .FirstOrDefault(dm => dm.IdDanhMuc == id);

            if (danhmuc == null)
                return NotFound();

            if (danhmuc.SanPhams.Any())
            {
                ModelState.AddModelError("", "Không thể xóa danh mục vì còn sản phẩm liên quan.");
                return View(danhmuc); // hoặc Redirect kèm TempData thông báo
            }

            // Xóa tất cả sản phẩm con trước
            _context.SanPhams.RemoveRange(danhmuc.SanPhams);
            _context.DanhMucs.Remove(danhmuc);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var danhmuc = _context.DanhMucs
                                  .Include(dm => dm.SanPhams)
                                  .FirstOrDefault(dm => dm.IdDanhMuc == id);

            if (danhmuc == null)
                return NotFound();

            return View(danhmuc);
        }



        public IActionResult QLDonHang(string trangthai)
        {
            var danhsachdonhang = _context.DonHangs.Include(x => x.IdKhNavigation).Include(x => x.ChiTietDonHangs)
                .ThenInclude(ct => ct.IdSpNavigation)
                  .Where(dh => dh.ChiTietDonHangs != null && dh.ChiTietDonHangs.Any())
                .OrderByDescending(x => x.NgayTao).ToList();
            if (!string.IsNullOrEmpty(trangthai))
            {
                danhsachdonhang = danhsachdonhang.Where(dh => dh.MaTrangThai == trangthai).ToList();
            }
            var kq = danhsachdonhang.OrderByDescending(x => x.NgayTao).ToList();
            //ViewBag.TrangThai = trangthai;

            return View(danhsachdonhang);
        }




        [HttpPost]
        public IActionResult CapNhatTrangThai(int idDonHang, string trangThaiMoi)
        {
            var donHang = _context.DonHangs.FirstOrDefault(x => x.IdDonHang == idDonHang);
            if (donHang == null)
            {
                return NotFound();
            }
            if (donHang.MaTrangThai == "Đã thanh toán" && trangThaiMoi != "Hoàn tiền")
            {
                TempData["ThongBao"] = $"❌ Đơn hàng #{idDonHang} đã thanh toán. Chỉ có thể chuyển sang trạng thái \"Hoàn tiền\".";
                return RedirectToAction("QLDonHang");
            }
            if (donHang.MaTrangThai == "Đã hủy" )
            {
                TempData["ThongBao"] = $"❌ Đơn hàng #{idDonHang} đã bị hủy và không thể cập nhật trạng thái.";
                return RedirectToAction("QLDonHang");
            }

        
            donHang.MaTrangThai = trangThaiMoi;
            _context.SaveChanges();
            TempData["ThongBao"] = $"✔️ Trạng thái đơn hàng #{idDonHang} đã được cập nhật thành \"{trangThaiMoi}\"";

            return RedirectToAction("QLDonHang");
        }





    }
}
