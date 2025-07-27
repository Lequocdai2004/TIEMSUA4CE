using System;
using System.Collections.Generic;

namespace BanSua4CE.Data;

public partial class User
{
    public int IdUser { get; set; }

    public string? NameUser { get; set; }

    public string? Password { get; set; }

    public string? Gmail { get; set; }

    public string? Role { get; set; }
}
