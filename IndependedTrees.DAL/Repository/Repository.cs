﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IndependedTrees.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> DbSet { get; private set; }
        protected DbContext DbContext { get; private set; }

        public Repository(IndependedTreesDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> filterExpression)
        {
            if(filterExpression != null)
            {
                return await filterExpression
                    .Compile()
                    .Invoke(DbSet)
                    .ToListAsync()
                    .ContinueWith(x => (IEnumerable<TEntity>)x.Result);
            }

            return await DbSet
                .ToListAsync()
                .ContinueWith(x => (IEnumerable<TEntity>)x);
        }

        public async Task<TEntity?> GetAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var result = DbSet.Remove(entity);
            if (result.State == EntityState.Deleted)
            {
                await DbContext.SaveChangesAsync();
            }
        }
    }
}
