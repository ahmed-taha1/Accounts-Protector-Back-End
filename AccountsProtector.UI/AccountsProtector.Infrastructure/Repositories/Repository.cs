using System.Linq.Expressions;
using AccountsProtector.AccountsProtector.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace AccountsProtector.AccountsProtector.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext.AppDbContext _db;
        
        public Repository(AppDbContext.AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] joins)
        {
            IQueryable<T> query = _db.Set<T>();
            foreach (var join in joins)
            {
                query = query.Include(join);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T oldEntity, T newEntity)
        {
            foreach (var prop in _db.Entry(oldEntity).Properties)
            {
                var original = prop.OriginalValue;
                var current = _db.Entry(newEntity).Property(prop.Metadata.Name).CurrentValue;
                if (current != null && (original == null || !original.Equals(current)))
                {
                    prop.CurrentValue = current;
                }
            }
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<T> SelectByMatchAsync(Expression<Func<T, bool>> match)
        {
            return await _db.Set<T>().SingleOrDefaultAsync(match);
        }

        public async Task<T> SelectByMatchAsync(Expression<Func<T, bool>> match, List<string> joins)
        {
            IQueryable<T> query = _db.Set<T>();

            // Include navigation properties specified in the includes array
            foreach (var join in joins)
            {
                query = query.Include(join);
            }

            // Apply the match predicate
            return (await query.FirstOrDefaultAsync(match))!;
        }

        public async Task<IEnumerable<T>> SelectListByMatchAsync(Expression<Func<T, bool>> match)
        {
            return await _db.Set<T>().Where(match).ToListAsync();
        }

        public async Task<IEnumerable<T>> SelectListByMatchAsync(Expression<Func<T, bool>> match, List<string> joins)
        {
            IQueryable<T> query = _db.Set<T>();

            // Include navigation properties specified in the includes array
            foreach (var join in joins)
            {
                query = query.Include(join);
            }

            // Apply the match predicate
            query = query.Where(match);

            return await query.ToListAsync();
        }
    }
}