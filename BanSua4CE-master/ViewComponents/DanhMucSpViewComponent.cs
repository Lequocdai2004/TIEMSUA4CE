using BanSua4CE.Data;
using BanSua4CE.Reponsitory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanSua4CE.ViewComponents
{
    public class DanhMucSpViewComponent: ViewComponent
    {
        private readonly ILoaiSPReponsitory _loaiSPReponsitory;
         
        //lấy danh mục
        public DanhMucSpViewComponent(ILoaiSPReponsitory loaiSPReponsitory)
        {
            _loaiSPReponsitory = loaiSPReponsitory;
           
        }

       
        public IViewComponentResult Invoke()
        {
            var danhMucList =  _loaiSPReponsitory.GetAllDanhMuc().OrderBy(x=>x.TenDanhMuc);
            return View(danhMucList);
        }
    }
}
