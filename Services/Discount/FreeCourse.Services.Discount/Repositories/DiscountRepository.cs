using Dapper;

namespace FreeCourse.Services.Discount.Repositories
{
    public class DiscountRepository : RepositoryBase<Models.Discount>, IDiscountRepository
    {
        public DiscountRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Task<Models.Discount> GetByCodeAndUserId(string code, string userId) => _dbConnection.QueryFirstOrDefaultAsync<Models.Discount>($"select * from {GetTableName()} where Code = @code and UserId = @userId", new { code, userId });
    }
}