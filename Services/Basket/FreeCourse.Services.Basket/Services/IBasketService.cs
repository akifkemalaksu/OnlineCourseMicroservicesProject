using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System.Text.Json;

namespace FreeCourse.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);

        Task<Response<bool>> SaveOrUpdate(BasketDto basket);

        Task<Response<bool>> Delete(string userId);
    }

    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDatabase().KeyDeleteAsync(userId);

            return status ?
                Response<bool>.Success(System.Net.HttpStatusCode.NoContent) :
                Response<bool>.Fail("Basket not found.", System.Net.HttpStatusCode.NotFound);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDatabase().StringGetAsync(userId);
            if (string.IsNullOrEmpty(existBasket))
                return Response<BasketDto>.Fail("Basket not found.", System.Net.HttpStatusCode.NotFound);

            var basket = JsonSerializer.Deserialize<BasketDto>(existBasket);
            return Response<BasketDto>.Success(basket, System.Net.HttpStatusCode.OK);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basket)
        {
            var status = await _redisService.GetDatabase().StringSetAsync(basket.UserId, JsonSerializer.Serialize(basket));
            return status ?
                Response<bool>.Success(System.Net.HttpStatusCode.NoContent) :
                Response<bool>.Fail("Basket could not update or save.", System.Net.HttpStatusCode.InternalServerError);
        }
    }
}