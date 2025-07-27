using BanSua4CE.Data;
using BanSua4CE.Helpers;
using BanSua4CE.Services;
using BanSua4CE.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BanSua4CE.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ShopBanSuaContext _context;
        private readonly IVnPayService _vnPayService;

        public GioHangController(ShopBanSuaContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

        const string CART_KEY = "MYCART";
        public List<GioHangVM> Cart => HttpContext.Session.Get<List<GioHangVM>>(CART_KEY) ?? new List<GioHangVM>();
        public IActionResult TrangGioHang()
        {
            return View(Cart);
        }


        public IActionResult ThemGioHang(int id, int soluong = 1)
        {
            var giohang = Cart;
            var item = giohang.SingleOrDefault(p => p.IdSp == id);
            if (item == null)
            {
                var sanpham = _context.SanPhams.SingleOrDefault(p => p.IdSp == id);
                if (sanpham == null)
                {
                    TempData["Message"] = $"Sản phẩm không tồn tại {id}";
                    return NotFound();
                }
                item = new GioHangVM
                {
                    IdSp = sanpham.IdSp,
                    TenSp = sanpham.TenSp,
                    DonGia = sanpham.DonGia,
                    HinhAnh = sanpham.HinhAnh,
                    SoLuong = soluong
                };
                giohang.Add(item);
            }
            else
            {
                item.SoLuong += soluong;
            }
            HttpContext.Session.Set(CART_KEY, giohang);
            return RedirectToAction("TrangGioHang");
        }


        public IActionResult XoaGioHang(int? id)
        {
            var giohang = Cart;
            if (giohang != null)
            {
                var item = giohang.SingleOrDefault(p => p.IdSp == id);
                if (item != null)
                {
                    giohang.Remove(item);
                    HttpContext.Session.Set(CART_KEY, giohang);
                }
            }
            return RedirectToAction("TrangGioHang");
        }


        public IActionResult GiamSoLuong(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var giohang = Cart;
            var item = giohang.FirstOrDefault(x => x.IdSp == id);
            if (item != null)
            {
                if (item.SoLuong > 1)
                {
                    item.SoLuong -= 1;
                }
                else
                {
                    giohang.Remove(item);
                }
            }
            HttpContext.Session.Set(CART_KEY, giohang);
            return RedirectToAction("TrangGioHang");

        }
        public IActionResult TangSoLuong(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var giohang = Cart;
            var item = giohang.FirstOrDefault(x => x.IdSp == id);
            if (item != null)
            {
                item.SoLuong += 1;
            }
            HttpContext.Session.Set(CART_KEY, giohang);
            return RedirectToAction("TrangGioHang");

        }

        [Authorize]
        [HttpGet]
        public IActionResult ThanhToan()
        {

            if (Cart.Count == 0)
            {
                return RedirectToAction("/");
            }
            return View(Cart);
        }


        [Authorize]
        [HttpPost]
        public IActionResult ThanhToan(ThanhToanVM model, string payment = "COD")
        {
            if (ModelState.IsValid)
            {
                var idkh = int.Parse(HttpContext.User.Claims.SingleOrDefault(x => x.Type == MySetting.CLAIM_CUSTOMERID).Value);
                var khachhang = new KhachHang();
                if (model.GiongKhachHang)
                {
                    khachhang = _context.KhachHangs.SingleOrDefault(x => x.IdKh == idkh);
                }
                var hoadon = new DonHang
                {
                    IdKh = idkh,
                    TenKh = model.TenKH,
                    DiaChi = model.DiaChi,
                    SoDienThoai = model.SoDienThoai,
                    NgayTao = DateTime.Now,
                    CachVanChuyen = "GRAB",
                    MaTrangThai = "Đang Xác Nhận",
                    GhiChu = model.GhiChu,
                    PhuongThucTraTien = payment == "Thanh toán VNPay" ? "VNPAY" : "COD"
                };
                if (payment == "Thanh toán VNPay")
                {
                    var vnPayModel = new VnPayRequestModel
                    {
                        Amount = Cart.Sum(p => p.ThanhTien),
                        CreatedDate = DateTime.Now,
                        Description = $"{model.TenKH} {model.SoDienThoai}",
                        FullName = model.TenKH,
                        OrderId = hoadon.IdDonHang,
                    };
                    return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
                }
                using (var transaction = _context.Database.BeginTransaction()) // Use 'using' for automatic disposal
                {
                    try
                    {
                        _context.AddRange(hoadon);
                        _context.SaveChanges();

                        var cthds = new List<ChiTietDonHang>();
                        foreach (var item in Cart)
                        {
                            cthds.Add(new ChiTietDonHang
                            {
                                IdDonHang = hoadon.IdDonHang,
                                SoLuong = item.SoLuong,
                                DonGia = item.DonGia,
                                IdSp = item.IdSp,
                                ThanhTien = item.SoLuong * item.DonGia
                            });
                        }

                        _context.ChiTietDonHangs.AddRange(cthds);
                        _context.SaveChanges();

                        //đặt hàng xong là giỏ hàng làm mới
                        HttpContext.Session.Set<List<GioHangVM>>(MySetting.CART_KEY, new List<GioHangVM>());
                        transaction.Commit(); // Commit only if all operations succeed
                        return View("ThanhCong");
                    }
                    catch
                    {
                        transaction.Rollback(); // Rollback if any exception occurs
                        TempData["Message"] = "Có lỗi xảy ra khi lưu đơn hàng.";
                        return RedirectToAction("PaymentFail");
                    }
                }

            }
            return View(Cart);
        }



        //[Authorize]
        public IActionResult ThanhCong()
        {
            return View();
        }

        //[Authorize]
        public IActionResult Success(VnPay responseModel)
        {

            // Lấy thông tin thanh toán từ TempData (ví dụ)
            if (responseModel == null)
            {
                TempData["Message"] = "Lỗi: Không nhận được thông tin thanh toán từ VNPAY.";
                return View("Error"); // Hoặc một view lỗi khác
            }

            // Truyền responseModel đến View Success
            return View(responseModel);
        }


        //[Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PaymentCallBack(ThanhToanVM model)
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            var staffId = int.Parse(HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value);
            var khachHang = new KhachHang();
            if (model.GiongKhachHang)
            {
                khachHang = _context.KhachHangs.SingleOrDefault(kh => kh.IdKh == staffId);
            }

            if (response.VnPayResponseCode == "00" && response.Success)
            {
                _context.Database.BeginTransaction();
                try
                {
                    // Tạo đơn hàng
                    var donHang = new DonHang
                    {
                        IdKh = staffId,
                        TenKh = model.TenKH ?? khachHang.TenKh, // Có thể lấy lại từ session nếu muốn
                        DiaChi = model.DiaChi ?? khachHang.DiaChi, // Hoặc lấy lại từ session
                        NgayTao = DateTime.Now,
                        CachVanChuyen = "GRAB",
                        PhuongThucTraTien = "VNPAY",
                        GhiChu = model.GhiChu,
                        MaTrangThai = "Đã thanh toán",
                    };

                    _context.DonHangs.Add(donHang);
                    _context.SaveChanges();

                    // Tạo chi tiết đơn hàng
                    var chiTietList = new List<ChiTietDonHang>();
                    foreach (var item in Cart)
                    {
                        var sp = _context.SanPhams.SingleOrDefault(x => x.IdSp == item.IdSp);

                        chiTietList.Add(new ChiTietDonHang
                        {
                            IdDonHang = donHang.IdDonHang,
                            IdSp = item.IdSp,
                            SoLuong = item.SoLuong,
                            DonGia = item.DonGia,
                            ThanhTien = item.DonGia * item.SoLuong
                        });
                    }

                    _context.UpdateRange(_context.SanPhams);
                    _context.AddRange(chiTietList);
                    _context.SaveChanges();

                    // Lưu giao dịch VNPay
                    var vnpay = new VnPay
                    {
                        TransactionId = response.TransactionId,
                        //OrderId = donHang.IdDonHang,
                        OrderId = response.OrderId,
                        DatedCreated = DateTime.Now,
                        OrderDescription = response.OrderDescription,
                        PaymentMethod = response.PaymentMethod
                    };
                    _context.VnPays.Add(vnpay);
                    _context.SaveChanges();

                    // Xoá giỏ hàng trong session

                    HttpContext.Session.Set<List<GioHangVM>>(MySetting.CART_KEY, new List<GioHangVM>());

                    _context.Database.CommitTransaction();
                    return View("Success", response);
                }
                catch (Exception ex)
                {
                    _context.Database.RollbackTransaction();
                    response.Success = false;
                    ModelState.AddModelError("", "Có lỗi khi xử lý đơn hàng: " + ex.Message);
                }
            }
            else
            {
                response.Success = false;
                if (string.IsNullOrEmpty(response.OrderDescription))
                {
                    response.OrderDescription = "Thanh toán bị hủy hoặc không thành công.";
                    return View("PaymentFail", response);
                }
            }

            return View("PaymentFail",response);
        }


    }




  




}
