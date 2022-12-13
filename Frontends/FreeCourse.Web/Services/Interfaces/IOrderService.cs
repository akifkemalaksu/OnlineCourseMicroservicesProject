using FreeCourse.Web.Models.Orders;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// synchronous data transfer 
        /// </summary>
        /// <param name="checkoutInfoInput"></param>
        /// <returns></returns>
        Task<OrderCreatedViewModel> CreateOrderAsync(CheckoutInfoInput checkoutInfoInput);

        /// <summary>
        /// asynchronous data transfer with RabbitMq
        /// </summary>
        /// <param name="checkoutInfoInput"></param>
        /// <returns></returns>
        Task<OrderSuspendViewModel> SuspendOrderAsync(CheckoutInfoInput checkoutInfoInput);

        Task<List<OrderViewModel>> GetOrderAsync();
    }
}
