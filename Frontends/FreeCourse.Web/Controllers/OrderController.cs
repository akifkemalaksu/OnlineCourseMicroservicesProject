using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.GetAsync();
            ViewBag.basket = basket;

            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
        {
            // async
            var orderSuspend = await _orderService.SuspendOrderAsync(checkoutInfoInput);
            if (!orderSuspend.IsSuccessful)
            {
                var basket = await _basketService.GetAsync();
                ViewBag.basket = basket;
                ViewBag.error = orderSuspend.Error;

                return View();
            }

            return RedirectToAction(nameof(SuccessfulCheckout), new OrderCreatedViewModel { IsSuccessful = true, OrderId = 0 });

            // sync çalışma
            var orderStatus = await _orderService.CreateOrderAsync(checkoutInfoInput);
            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.GetAsync();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;

                return View();
            }

            return RedirectToAction(nameof(SuccessfulCheckout), orderStatus);
        }

        public IActionResult SuccessfulCheckout(OrderCreatedViewModel orderCreatedViewModel)
        {
            return View(orderCreatedViewModel);
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            var order = await _orderService.GetOrderAsync();
            return View(order);
        }
    }
}
