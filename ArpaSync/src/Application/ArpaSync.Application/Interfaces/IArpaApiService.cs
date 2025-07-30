using ArpaSync.Application.DTOs.Arpa;

namespace ArpaSync.Application.Interfaces;

public interface IArpaApiService
{
    /// <summary>
    /// چک کردن وجود مشتری در آرپا و در صورت عدم وجود ایجاد آن
    /// </summary>
    /// <param name="customerData">اطلاعات مشتری</param>
    /// <returns>BusinessId مشتری در آرپا</returns>
    Task<string> EnsureCustomerExistsAsync(ArpaCustomerDto customerData);
    
    /// <summary>
    /// ارسال اطلاعات سفارش به آرپا
    /// </summary>
    /// <param name="orderData">اطلاعات سفارش</param>
    /// <returns>شناسه سفارش در آرپا</returns>
    Task<string> SendOrderToArpaAsync(ArpaOrderDto orderData);
    
    /// <summary>
    /// ارسال اطلاعات پرداخت به آرپا
    /// </summary>
    /// <param name="paymentData">اطلاعات پرداخت</param>
    /// <returns>شناسه پرداخت در آرپا</returns>
    Task<string> SendPaymentToArpaAsync(ArpaPaymentDto paymentData);
}