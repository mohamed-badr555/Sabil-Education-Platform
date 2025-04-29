using DAL;
using DAL.Data.Models;
using DAL.Data.Repository.Models;
using DAL.DB_Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly E_LearningDB _dbContext;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(E_LearningDB dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
           => await ApplySpecification(spec).ToListAsync();


        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
           => await ApplySpecification(spec).FirstOrDefaultAsync();


        public async Task<int> GetCountAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).CountAsync();

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AnyAsync(predicate);
        }

    }
}
