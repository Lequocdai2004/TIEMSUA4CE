using AutoMapper;
using BanSua4CE.Data;
using BanSua4CE.ViewModels;

namespace BanSua4CE.Helpers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DangKyVM, KhachHang>();
        }
    }
}
