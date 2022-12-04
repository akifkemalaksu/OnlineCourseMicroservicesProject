﻿using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Discounts;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscountAsync(string discountCode)
        {
            var response = await _httpClient.GetFromJsonAsync<Response<DiscountViewModel>>($"getbycode/{discountCode}");
            //todo loglama işlemi
            if (!response.IsSuccessful)
                return null;

            return response.Data;
        }
    }
}
