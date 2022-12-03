using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddBasketItemAsync(BasketItemViewModel basketItemViewModel)
        {
            BasketViewModel basket = await GetAsync();
            if (basket is null)
            {
                basket = new BasketViewModel();
                basket.BasketItems.Add(basketItemViewModel);
            }

            if (!basket.BasketItems.Any(x => x.CourseId == basketItemViewModel.CourseId))
                basket.BasketItems.Add(basketItemViewModel);

            await SaveOrUpdateAsync(basket);
        }

        public Task<bool> ApplyDiscountAsync(string discountCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelApplyDiscountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync()
        {
            var response = await _httpClient.DeleteAsync("baskets");
            return response.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> GetAsync()
        {
            var resonse = await _httpClient.GetFromJsonAsync<Response<BasketViewModel>>("baskets");
            if (resonse is null)
                return null;

            return resonse.Data;
        }

        public async Task<bool> RemoveBasketItemAsync(string courseId)
        {
            var basket = await GetAsync();
            if (basket is null)
                return false;

            var deleteBasketItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);
            if (deleteBasketItem is null)
                return false;

            var deleteResult = basket.BasketItems.Remove(deleteBasketItem);

            if (!deleteResult)
                return deleteResult;

            if (basket.BasketItems.Any())
                basket.DiscountCode = null;

            return await SaveOrUpdateAsync(basket);
        }

        public async Task<bool> SaveOrUpdateAsync(BasketViewModel basketViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync("baskets", basketViewModel);
            return response.IsSuccessStatusCode;
        }
    }
}
