using System;
using System.Collections.Generic;

namespace BanSua4CE.Data;

public partial class ChiTietDonHang
{
    public int IdChiTietDh { get; set; }

    public double DonGia { get; set; }

    public int SoLuong { get; set; }

    public double ThanhTien { get; set; }

    public int? IdSp { get; set; }

    public int? IdDonHang { get; set; }

    public virtual DonHang? IdDonHangNavigation { get; set; }

    public virtual SanPham? IdSpNavigation { get; set; }
}
