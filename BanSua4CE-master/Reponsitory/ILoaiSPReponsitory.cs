using BanSua4CE.Data;

namespace BanSua4CE.Reponsitory
{
    public interface ILoaiSPReponsitory
    {
        DanhMuc Add(DanhMuc loai);
        DanhMuc Update(DanhMuc loai);
        DanhMuc Delete(int id);
        
        DanhMuc GetDanhMucSp(int id);

        IEnumerable<DanhMuc> GetAllDanhMuc();
    }

}
