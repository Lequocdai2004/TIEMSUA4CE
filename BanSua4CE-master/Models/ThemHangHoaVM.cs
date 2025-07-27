namespace BanSua4CE.Models
{
    public class ThemHangHoaVM
    {
        public int IdSp { get; set; }
        public string TenSp { get; set; } = null!;
        public string? MoTa { get; set; }
        public decimal DonGia { get; set; }
        public string? XuatXu { get; set; }
        public string? HinhAnh { get; set; }
        public bool Status { get; set; }
        public int IdDanhMuc { get; set; }
        public string Hinh { get; set; } = null!;
    }
}
