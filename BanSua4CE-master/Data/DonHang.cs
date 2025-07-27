using System;
using System.Collections.Generic;

namespace BanSua4CE.Data;

public partial class DonHang
{
    public int IdDonHang { get; set; }

    public string? TenKh { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? GhiChu { get; set; }

    public string? PhuongThucTraTien { get; set; }

    public string? CachVanChuyen { get; set; }

    public string? MaTrangThai { get; set; }

    public int? IdKh { get; set; }

    public int? IdSp { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual KhachHang? IdKhNavigation { get; set; }

    public virtual SanPham? IdSpNavigation { get; set; }
}
