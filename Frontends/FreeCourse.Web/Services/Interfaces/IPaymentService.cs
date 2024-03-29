﻿using FreeCourse.Web.Models.Payments;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReceivePaymentAsync(PaymentInfoInput paymentInfoInput);
    }
}
