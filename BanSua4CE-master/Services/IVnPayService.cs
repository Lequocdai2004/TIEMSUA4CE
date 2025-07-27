using BanSua4CE.Data;
using BanSua4CE.ViewModels;

namespace BanSua4CE.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPayRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
