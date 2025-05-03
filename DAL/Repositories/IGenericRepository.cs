using DAL;
using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(bool includeDeleted = false);
        Task<T?> GetByIdAsync(string id, bool includeDeleted = false);
        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SoftDeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
