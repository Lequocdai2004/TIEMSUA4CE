using System;
using System.Collections.Generic;

namespace BanSua4CE.Data;

public partial class LienHe
{
    public int IdLienHe { get; set; }

    public string? HoTen { get; set; }

    public string? Email { get; set; }

    public string? NoiDung { get; set; }

    public DateTime? CreatedAt { get; set; }
}
