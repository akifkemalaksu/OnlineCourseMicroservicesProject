using FreeCourse.Services.Discount.Repositories;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var discount = await _discountRepository.GetById(id);

            if (discount is null)
                return Response<NoContent>.Fail("Discount is not found by given id.", System.Net.HttpStatusCode.NotFound);

            var status = await _discountRepository.Delete(discount);

            if (status)
                return Response<NoContent>.Success(System.Net.HttpStatusCode.NoContent);

            return Response<NoContent>.Fail("An error accured while updating.", System.Net.HttpStatusCode.InternalServerError);
        }

        public async Task<Response<IEnumerable<Models.Discount>>> GetAll()
        {
            var discounts = await _discountRepository.GetAll();
            return Response<IEnumerable<Models.Discount>>.Success(discounts, System.Net.HttpStatusCode.OK);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = await _discountRepository.GetByCodeAndUserId(code, userId);

            if (discount is null)
                return Response<Models.Discount>.Fail("Discount not found", System.Net.HttpStatusCode.NotFound);

            return Response<Models.Discount>.Success(discount, System.Net.HttpStatusCode.OK);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = await _discountRepository.GetById(id);

            if (discount is null)
                return Response<Models.Discount>.Fail("Discount not found", System.Net.HttpStatusCode.NotFound);

            return Response<Models.Discount>.Success(discount, System.Net.HttpStatusCode.OK);
        }

        public async Task<Response<NoContent>> Add(Models.Discount discount)
        {
            var status = await _discountRepository.Add(discount);

            if (status > 0)
                return Response<NoContent>.Success(System.Net.HttpStatusCode.NoContent);

            return Response<NoContent>.Fail("An error accured while adding.", System.Net.HttpStatusCode.InternalServerError);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var status = await _discountRepository.Update(discount);

            if (status)
                return Response<NoContent>.Success(System.Net.HttpStatusCode.NoContent);

            return Response<NoContent>.Fail("Discount is not found.", System.Net.HttpStatusCode.NotFound);
        }
    }
}