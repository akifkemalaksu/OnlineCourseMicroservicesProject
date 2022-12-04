using FluentValidation;
using FreeCourse.Web.Models.Catalogs;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator : AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("İsim alanı boş olamaz.");
            RuleFor(c => c.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz.");
            RuleFor(c => c.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş olamaz.");
            RuleFor(c => c.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz.").ScalePrecision(2, 6).WithMessage("Hatalı para birimi.");
            RuleFor(c => c.CategoryId).NotEmpty().WithMessage("Kategori alanı boş olamaz.");
        }
    }
}
