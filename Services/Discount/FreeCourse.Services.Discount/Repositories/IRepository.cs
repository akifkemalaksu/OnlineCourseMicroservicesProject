using FreeCourse.Services.Discount.Models;

namespace FreeCourse.Services.Discount.Repositories
{
    public interface IRepository<T>
        where T : class
    {
        Task<int> Add(T entity);

        Task<bool> Delete(T entity);

        Task<bool> Update(T entity);

        Task<T> GetById(int id);

        Task<IEnumerable<T>> GetAll();
    }
}