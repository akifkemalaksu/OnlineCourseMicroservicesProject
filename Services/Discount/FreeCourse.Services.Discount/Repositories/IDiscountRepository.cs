using Dapper.Contrib.Extensions;

namespace FreeCourse.Services.Discount.Repositories
{
    public interface IDiscountRepository : IRepository<Discount.Models.Discount>
    {
        Task<Discount.Models.Discount> GetByCodeAndUserId(string code, string userId);
    }
}