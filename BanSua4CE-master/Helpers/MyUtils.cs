namespace BanSua4CE.Helpers
{
    public class MyUtils
    {
        public static string UploadHinh(IFormFile hinhanh, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "hinhanh", folder);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    hinhanh.CopyTo(myfile);
                }
                return hinhanh.FileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


    }
}
