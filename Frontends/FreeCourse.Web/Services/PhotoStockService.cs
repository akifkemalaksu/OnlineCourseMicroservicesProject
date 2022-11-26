using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.PhotoStocks;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhotoAsync(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos/{photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhotoAsync(IFormFile photo)
        {
            if (photo is null || photo.Length <= 0)
                return null; //todo uyarı dön

            var randomFileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(photo.FileName));

            using var ms = new MemoryStream();

            await photo.CopyToAsync(ms);

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(ms.ToArray()), "photo", randomFileName);

            var response = await _httpClient.PostAsync("photos", multipartContent);

            if (!response.IsSuccessStatusCode)
                return null; //todo uyarı dön

            var responseViewModel = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return responseViewModel.Data;
        }
    }
}
