using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalogs
{
    public class CourseCreateInput
    {
        [Display(Name = "Kurs ismi")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Kurs açıklama")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Kurs fiyatı")]
        [Required]
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Photo { get; set; }
        public FeatureViewModel Feature { get; set; }
        [Display(Name = "Kurs kategorisi")]
        [Required]
        public string CategoryId { get; set; }
        [Display(Name = "Kurs resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
