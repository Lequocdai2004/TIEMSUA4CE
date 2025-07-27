using BanSua4CE.Data;
using BanSua4CE.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BanSua4CE.Controllers
{
    public class HomeController : Controller
    {
        private ShopBanSuaContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ShopBanSuaContext db)
        {
            _logger = logger;
            _db = db;
        }


        //danh sách sản phẩm
        public IActionResult Index()
        {
            var danhsachsp = _db.SanPhams.ToList();
            return View(danhsachsp);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GioiThieuVeTiemSua()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult KienThucNuoiCon()
        {
            return View();
        }

        public IActionResult ChinhSachBaoMat()
        {
            return View();
        }

        public IActionResult QuyDinhDoiTraHang()
        {
            return View();
        }

        public IActionResult PhuongThucGiaoHang()
        {
            return View();
        }

        public IActionResult PhuongThucThanhToan()
        {
            return View();
        }

        public IActionResult ChinhSachBaoHanh()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("HoTen", "Email", "NoiDung", "CreatedAt")] LienHe lienhe)
        {
            if (ModelState.IsValid)
            {
                _db.LienHes.Add(lienhe);
                await _db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Gửi liên hệ thành công";

            }
            return View(lienhe);
        }


        public IActionResult GioiThieu()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
