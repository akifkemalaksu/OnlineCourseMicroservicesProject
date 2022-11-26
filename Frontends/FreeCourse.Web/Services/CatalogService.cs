using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalogs;
using FreeCourse.Web.Services.Interfaces;
using System.Text.Json;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }

        public async Task<List<CourseViewModel>> GetAllCoursesAsync()
        {
            var response = await _httpClient.GetAsync("courses");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            Parallel.ForEach(responseSuccess.Data, (course) => course.PhotoWithUrl = _photoHelper.GetPhotoStockUrl(course.Photo));
            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"courses/getallbyuserid/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            Parallel.ForEach(responseSuccess.Data, (course) => course.PhotoWithUrl = _photoHelper.GetPhotoStockUrl(course.Photo));
            return responseSuccess.Data;
        }

        public async Task<CourseViewModel> GetCourseByIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/{courseId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            responseSuccess.Data.PhotoWithUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.Photo);
            return responseSuccess.Data;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync($"categories");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return responseSuccess.Data;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var responsePhoto = await _photoStockService.UploadPhotoAsync(courseCreateInput.PhotoFormFile);
            if (responsePhoto is not null)
                courseCreateInput.Photo = responsePhoto.Url;
            //todo logging tüm servislerde

            var response = await _httpClient.PostAsJsonAsync("courses", courseCreateInput);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"courses/{courseId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var responsePhoto = await _photoStockService.UploadPhotoAsync(courseUpdateInput.PhotoFormFile);
            if (responsePhoto is not null)
            {
                await _photoStockService.DeletePhotoAsync(courseUpdateInput.Photo);
                courseUpdateInput.Photo = responsePhoto.Url;
            }

            var response = await _httpClient.PutAsJsonAsync("courses", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
