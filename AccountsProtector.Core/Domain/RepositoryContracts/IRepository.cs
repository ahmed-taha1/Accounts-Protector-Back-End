using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params string[] joins);
        Task<T> GetByIdAsync(int id);
        Task InsertAsync (T entity);
        Task UpdateAsync (T oldEntity, T newEntity);
        Task DeleteAsync (T entity);
        Task<T> SelectByMatch (Expression<Func<T, bool>> match);
        Task<IEnumerable<T>> SelectListByMatchAsync(Expression<Func<T, bool>> match);
    }
}
