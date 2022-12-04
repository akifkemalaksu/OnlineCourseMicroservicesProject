using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FreeCourse.Web.Models.Catalogs
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }
        [Display(Name = "Kurs ismi")]
        public string Name { get; set; }
        [Display(Name = "Kurs açıklama")]
        public string Description { get; set; }
        [Display(Name = "Kurs fiyatı")]
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Photo { get; set; }
        public FeatureViewModel Feature { get; set; }
        [Display(Name = "Kurs kategorisi")]
        public string CategoryId { get; set; }
        [Display(Name = "Kurs resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
