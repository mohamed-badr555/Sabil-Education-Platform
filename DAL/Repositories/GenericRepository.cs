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

        public async Task<List<T>> GetAllAsync(bool includeDeleted = false)
        {
            return await _dbSet
                .Where(e => includeDeleted || !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id, bool includeDeleted = false)
        {
            return await _dbSet
                .Where(e => e.Id == id && (includeDeleted || !e.IsDeleted))
                .FirstOrDefaultAsync();
        }

        // The existing GetByIdAsync to maintain compatibility
        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            // Apply the specification and filter out deleted items
            var query = ApplySpecification(spec);
            if (!spec.IncludeDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            // Apply the specification and filter out deleted items
            var query = ApplySpecification(spec);
            if (!spec.IncludeDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            // Apply the specification and filter out deleted items
            var query = ApplySpecification(spec);
            if (!spec.IncludeDeleted)
            {
                query = query.Where(e => !e.IsDeleted);
            }
            return await query.CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }

        public async Task InsertAsync(T entity)
        {
            // Generate a unique ID if it's not provided
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
            }

            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        // Hard delete
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Soft delete
        public async Task SoftDeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AnyAsync(predicate);
        }
    }
}
