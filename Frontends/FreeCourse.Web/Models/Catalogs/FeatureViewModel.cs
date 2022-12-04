using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FreeCourse.Web.Models.Catalogs
{
    public class FeatureViewModel
    {
        [Display(Name = "Kurs süresi")]
        public int Duration { get; set; }
    }
}
