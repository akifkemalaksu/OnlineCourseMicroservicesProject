using AutoMapper;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Models.Payments;
using FreeCourse.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly ICatalogService _catalogService;
        private readonly PhotoHelper _photoHelper;
        private readonly IMapper _mapper;

        public OrderService(HttpClient httpClient, IPaymentService paymentService, IBasketService basketService, ISharedIdentityService sharedIdentityService, ICatalogService catalogService, PhotoHelper photoHelper, IMapper mapper)
        {
            _httpClient = httpClient;
            _paymentService = paymentService;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
            _catalogService = catalogService;
            _photoHelper = photoHelper;
            _mapper = mapper;
        }

        public async Task<OrderCreatedViewModel> CreateOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.GetAsync();

            var payment = _mapper.Map<PaymentInfoInput>(checkoutInfoInput);
            payment.TotalPrice = basket.TotalPrice;

            var responsePayment = await _paymentService.ReceivePaymentAsync(payment);

            if (!responsePayment)
                return new OrderCreatedViewModel { IsSuccessful = false, Error = "Ödeme alınamadı." };

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = _mapper.Map<AddressCreateInput>(checkoutInfoInput),
            };

            var tasks = basket.BasketItems.Select(async item =>
            {
                var course = await _catalogService.GetCourseByIdAsync(item.CourseId);

                var orderItem = new OrderItemCreateInput
                {
                    ProductId = item.CourseId,
                    Price = item.GetCurrentPrice,
                    PictureUrl = _photoHelper.GetPhotoStockUrl(course.Photo),
                    ProductName = item.CourseName
                };

                orderCreateInput.OrderItems.Add(orderItem);
            });

            await Task.WhenAll(tasks);

            var response = await _httpClient.PostAsJsonAsync("orders", orderCreateInput);
            if (!response.IsSuccessStatusCode)
                return new OrderCreatedViewModel { IsSuccessful = false, Error = "Sipariş oluşturulamadı." };

            var orderCreated = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            orderCreated.Data.IsSuccessful = true;

            await _basketService.DeleteAsync();

            return orderCreated.Data;
        }

        public async Task<List<OrderViewModel>> GetOrderAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return response.Data;
        }

        public async Task<OrderSuspendViewModel> SuspendOrderAsync(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.GetAsync();

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = _mapper.Map<AddressCreateInput>(checkoutInfoInput),
            };

            var tasks = basket.BasketItems.Select(async item =>
            {
                var course = await _catalogService.GetCourseByIdAsync(item.CourseId);

                var orderItem = new OrderItemCreateInput
                {
                    ProductId = item.CourseId,
                    Price = item.GetCurrentPrice,
                    PictureUrl = _photoHelper.GetPhotoStockUrl(course.Photo),
                    ProductName = item.CourseName
                };

                orderCreateInput.OrderItems.Add(orderItem);
            });
            await Task.WhenAll(tasks);

            var payment = _mapper.Map<PaymentInfoInput>(checkoutInfoInput);
            payment.TotalPrice = basket.TotalPrice;
            payment.Order = orderCreateInput;

            var responsePayment = await _paymentService.ReceivePaymentAsync(payment);

            if (!responsePayment)
                return new OrderSuspendViewModel
                {
                    Error = "Ödeme alınamadı.",
                    IsSuccessful = false
                };

            await _basketService.DeleteAsync();

            return new OrderSuspendViewModel
            {
                IsSuccessful = true
            };
        }
    }
}
