using System;
using System.Collections.Generic;

namespace BanSua4CE.Data;

public partial class KhachHang
{
    public int IdKh { get; set; }

    public string TenKh { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string MatKhau { get; set; } = null!;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
