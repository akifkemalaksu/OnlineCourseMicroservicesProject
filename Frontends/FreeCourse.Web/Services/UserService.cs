using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using System.Drawing.Text;

namespace FreeCourse.Web.Services
{
    public class UserService : IUserService
    {
        public readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser() => await _httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser");
    }
}