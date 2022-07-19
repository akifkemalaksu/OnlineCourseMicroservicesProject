using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityServices;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityServices)
        {
            _discountService = discountService;
            _sharedIdentityServices = sharedIdentityServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _discountService.GetAll();
            return CreateActionResultInstance(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _discountService.GetById(id);
            return CreateActionResultInstance(result);
        }

        [HttpGet("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var result = await _discountService.GetByCodeAndUserId(code, _sharedIdentityServices.GetUserId);
            return CreateActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Models.Discount discount)
        {
            var result = await _discountService.Add(discount);
            return CreateActionResultInstance(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Models.Discount discount)
        {
            var result = await _discountService.Update(discount);
            return CreateActionResultInstance(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _discountService.Delete(id);
            return CreateActionResultInstance(result);
        }
    }
}