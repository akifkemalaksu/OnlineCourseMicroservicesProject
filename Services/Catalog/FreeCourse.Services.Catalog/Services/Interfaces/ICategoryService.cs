using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Catalog.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<Response<IEnumerable<CategoryDto>>> GetAllAsync();

        public Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);

        public Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}