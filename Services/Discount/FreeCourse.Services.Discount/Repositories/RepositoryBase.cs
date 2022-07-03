using Dapper;
using Dapper.Contrib.Extensions;
using Npgsql;
using System.Data;
using System.Reflection;

namespace FreeCourse.Services.Discount.Repositories
{
    public class RepositoryBase<T> : IRepository<T>, IDisposable
        where T : class
    {
        protected readonly IDbConnection _dbConnection;
        private bool _disposed;

        public RepositoryBase(IConfiguration configuration)
        {
            _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));

            DapperExtensions.DapperAsyncExtensions.SqlDialect = new DapperExtensions.Sql.PostgreSqlDialect();
        }

        public Task<int> Add(T entity) => _dbConnection.InsertAsync(entity);

        public Task<bool> Delete(T entity) => _dbConnection.DeleteAsync(entity);

        public Task<bool> Update(T entity) => _dbConnection.UpdateAsync(entity);

        public Task<IEnumerable<T>> GetAll() => _dbConnection.GetAllAsync<T>();

        public Task<T> GetById(int id) => _dbConnection.GetAsync<T>(id);

        protected string GetTableName()
        {
            var tableName = typeof(T).GetCustomAttribute<Dapper.Contrib.Extensions.TableAttribute>(false)?.Name;
            return tableName ?? nameof(T);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbConnection.Dispose();
            }

            _disposed = true;
        }
    }
}