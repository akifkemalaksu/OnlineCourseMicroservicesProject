using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<IEnumerable<Models.Discount>>> GetAll();

        Task<Response<Models.Discount>> GetById(int id);

        Task<Response<NoContent>> Add(Models.Discount discount);

        Task<Response<NoContent>> Update(Models.Discount discount);

        Task<Response<NoContent>> Delete(int id);

        Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId);
    }
}