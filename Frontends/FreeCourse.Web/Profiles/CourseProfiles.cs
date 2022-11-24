using AutoMapper;
using FreeCourse.Web.Models.Catalogs;

namespace FreeCourse.Web.Profiles
{
    public class CourseProfiles : Profile
    {
        public CourseProfiles()
        {
            CreateMap<CourseViewModel, CourseUpdateInput>();
        }
    }
}
