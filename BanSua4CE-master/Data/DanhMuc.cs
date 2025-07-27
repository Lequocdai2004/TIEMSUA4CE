using System;
using System.Collections.Generic;

namespace BanSua4CE.Data;

public partial class DanhMuc
{
    public int IdDanhMuc { get; set; }

    public string? TenDanhMuc { get; set; }

    public bool? TrangThai { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
