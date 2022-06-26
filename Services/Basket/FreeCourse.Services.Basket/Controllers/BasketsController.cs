using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityServices _sharedIdentityServices;

        public BasketsController(IBasketService basketService, ISharedIdentityServices sharedIdentityServices)
        {
            _basketService = basketService;
            _sharedIdentityServices = sharedIdentityServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var basket = await _basketService.GetBasket(_sharedIdentityServices.GetUserId);
            return CreateActionResultInstance(basket);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basket)
        {
            var response = await _basketService.SaveOrUpdate(basket);
            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            var response = await _basketService.Delete(_sharedIdentityServices.GetUserId);
            return CreateActionResultInstance(response);
        }
    }
}