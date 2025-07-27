
using BanSua4CE.Data;
using BanSua4CE.Helpers;
using BanSua4CE.Reponsitory;
using BanSua4CE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//sử dụng automapper cho đăng nhập đăng ký mã hóa mật khẩu
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
//




//gio hang
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(70);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//gio hang


builder.Services.AddDbContext<ShopBanSuaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopSua")));


//danh mục sản phẩm
//var connection = builder.Configuration.GetConnectionString("ShopBanSuaContext");
//builder.Services.AddDbContext<ShopBanSuaContext>(options => options.UseSqlServer(connection));


builder.Services.AddScoped<ILoaiSPReponsitory, LoaiSPReponsitory>();
//danh mục sản phẩm

// Thêm dịch vụ xác thực và phân quyền
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/TaiKhoan/DangNhap";  // Trang đăng nhập
        options.AccessDeniedPath = "/AccessDenied"; // Trang truy cập bị từ chối
    });

builder.Services.AddAuthorization();


//builder.Environment.EnvironmentName = Environments.Production;

//đăng ký VnPay

builder.Services.AddSingleton<IVnPayService, VnPayService>();
//


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Trang lỗi tùy chỉnh
    app.UseHsts(); // Bắt buộc dùng HTTPS
}





app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0
app.UseSession(); //su dung gio hang

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();





