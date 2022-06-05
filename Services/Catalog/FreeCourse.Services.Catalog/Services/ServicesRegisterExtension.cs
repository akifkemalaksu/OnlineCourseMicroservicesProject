using FreeCourse.Services.Catalog.Services.Interfaces;

namespace FreeCourse.Services.Catalog.Services
{
    internal static class ServicesRegisterExtension
    {
        internal static IServiceCollection ServicesRegister(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();

            return services;
        }
    }
}