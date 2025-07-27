using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanSua4CE.ViewModels
{
    public class DangKyVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdKh { get; set; }



        [Required(ErrorMessage = "Vui lòng nhập Tên Khách Hàng")]
        public string TenKh { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;



        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        public string MatKhau { get; set; } = null!;


        [Display(Name = "Địa Chỉ")]
        public string? DiaChi { get; set; }

        [Display(Name = "Số Điện Thoại")]
        public string? SoDienThoai { get; set; }


    }
}
