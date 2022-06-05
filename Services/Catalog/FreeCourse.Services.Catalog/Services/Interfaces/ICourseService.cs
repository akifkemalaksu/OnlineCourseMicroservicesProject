using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Catalog.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Response<IEnumerable<CourseDto>>> GetAllAsync();

        Task<Response<CourseDto>> GetByIdAsync(string id);

        Task<Response<IEnumerable<CourseDto>>> GetAllByUserIdAsync(string userId);

        Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreate);

        Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdate);

        Task<Response<NoContent>> DeleteAsync(string id);
    }
}