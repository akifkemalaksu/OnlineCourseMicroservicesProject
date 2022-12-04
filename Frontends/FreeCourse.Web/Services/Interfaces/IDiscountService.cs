using FreeCourse.Web.Models.Discounts;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscountAsync(string discountCode);
    }
}
