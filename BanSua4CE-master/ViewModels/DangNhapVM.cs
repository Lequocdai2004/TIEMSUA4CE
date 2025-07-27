using System.ComponentModel.DataAnnotations;

namespace BanSua4CE.ViewModels
{
    public class DangNhapVM
    {
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
    }
}
