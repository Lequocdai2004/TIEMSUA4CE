using System;
using System.Collections.Generic;

namespace BanSua4CE.Data;

public partial class SanPham
{
    public int IdSp { get; set; }

    public string TenSp { get; set; } = null!;

    public double DonGia { get; set; }

    public string? MoTa { get; set; }

    public string? XuatXu { get; set; }

    public string? HinhAnh { get; set; }

    public bool? Status { get; set; }

    public int? IdDanhMuc { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual DanhMuc? IdDanhMucNavigation { get; set; }
}
