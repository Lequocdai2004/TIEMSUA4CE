using BanSua4CE.Data;

namespace BanSua4CE.Reponsitory
{
    public class LoaiSPReponsitory : ILoaiSPReponsitory
    {


        private readonly ShopBanSuaContext _context;
        public LoaiSPReponsitory(ShopBanSuaContext context)
        {
            _context = context;
        }



        public DanhMuc Add(DanhMuc loai)
        {
            _context.DanhMucs.Add(loai);
            _context.SaveChanges();
            return loai;
        }

        public DanhMuc Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DanhMuc> GetAllDanhMuc()
        {
           return _context.DanhMucs.ToList();
           //return _context.DanhMucs;

        }

        public DanhMuc GetDanhMucSp(int id)
        {
            return _context.DanhMucs.Find(id);
        }

        public DanhMuc Update(DanhMuc loai)
        {
            _context.Update(loai);
            _context.SaveChanges();
            return loai;
        }
    }
}
