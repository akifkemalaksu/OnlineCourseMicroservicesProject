using FreeCourse.Web.Models.Catalogs;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCoursesAsync();
        Task<List<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId);
        Task<CourseViewModel> GetCourseByIdAsync(string courseId);
        Task<List<CategoryViewModel>> GetAllCategoriesAsync();
        Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput);
        Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);
        Task<bool> DeleteCourseAsync(string courseId);
    }
}
