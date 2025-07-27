using BanSua4CE.Data;
using BanSua4CE.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Core;
using System.Security.Cryptography.Xml;
using X.PagedList;

namespace BanSua4CE.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        ShopBanSuaContext db = new ShopBanSuaContext();
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            
        }


        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listproducts = db.SanPhams.AsNoTracking().Where(sp=>sp.Status == true).OrderBy(x => x.TenSp);
            PagedList<SanPham> products = new PagedList<SanPham>(listproducts, pageNumber, pageSize);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

       
        public IActionResult SanPhamTheoDM (int? id,int? page)
        {
            if (id == null || !db.DanhMucs.Any(c => c.IdDanhMuc == id))
            {
                return NotFound("Không tìm thấy danh mục sản phẩm này");
            }
            int pageSize = 8;

            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listsp = db.SanPhams.Where(x=>x.IdDanhMuc == id).Where(sp => sp.Status == true).OrderBy(x => x.TenSp).ToList();
            PagedList<SanPham> sanphams = new PagedList<SanPham>(listsp, pageNumber, pageSize);
            return View(sanphams);
        }


        [Route("Product/ChiTietSanPham/{id?}")]
        public IActionResult ChiTietSanPham(int id)
        {
            var sanphamtheoid = db.SanPhams.Include(x=>x.IdDanhMucNavigation).SingleOrDefault(x=>x.IdSp==id);
            //ViewBag.sanpham = sanphamtheoid;
            if(sanphamtheoid == null)
            {
                return NotFound("Không tìm thấy sản phẩm này");
            }
            var model = new SanPhamVM()
            {
                IdSp = sanphamtheoid.IdSp,
                TenSp = sanphamtheoid.TenSp,
                DonGia = sanphamtheoid.DonGia,
                XuatXu = sanphamtheoid.XuatXu,
                HinhAnh = sanphamtheoid.HinhAnh,
                MoTa = sanphamtheoid.MoTa,
                IdDanhMuc = sanphamtheoid.IdDanhMuc,
                TenDanhMuc = sanphamtheoid.IdDanhMucNavigation.TenDanhMuc,
            };
            return View(model);
        }


        public IActionResult TimKiemSP(string? query)
        {
            var listsp = db.SanPhams.AsQueryable();

            if (query != null)
            {
                listsp = listsp.Where(p => p.TenSp.Contains(query));
            }

            var ketqua = listsp.Select(p => new SanPhamVM()
            {
                IdSp = p.IdSp,
                TenSp = p.TenSp,
                DonGia = p.DonGia,
                XuatXu = p.XuatXu,
                HinhAnh = p.HinhAnh,
                MoTa = p.MoTa,
                IdDanhMuc = p.IdDanhMuc,
                TenDanhMuc = p.IdDanhMucNavigation.TenDanhMuc
            }).ToList();


            ViewBag.TuKhoa = query;
            return View(ketqua);
          
        }

    }
}
